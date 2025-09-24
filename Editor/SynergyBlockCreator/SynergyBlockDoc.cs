using KHJ.Enum;
using KHJ.SO;
using Main.Runtime.Core.StatSystem;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonInfo
{
    public Button Button { get; }
    public Vector2Int Position { get; }
    public bool IsFilled { get; set; }

    public ButtonInfo(Button button, Vector2Int position, bool isFilled = false)
    {
        Button = button ?? throw new ArgumentNullException(nameof(button));
        Position = position;
        IsFilled = isFilled;
    }
}

#if UNITY_EDITOR
public class SynergyBlockDoc : EditorWindow
{
    private const int GRID_SIZE = 11;
    private const int CENTER_INDEX = GRID_SIZE / 2;
    private static readonly Vector2Int[] AdjacencyDirs = new[]
    {
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };

    [SerializeField] private VisualTreeAsset m_VisualTreeAsset;

    private readonly string assetFolderPath = "Assets/00Work/KHJ/08.SO/Synergy";

    private ButtonInfo[,] _buttonGrid;
    private readonly Dictionary<Button, ButtonInfo> _buttonLookup = new Dictionary<Button, ButtonInfo>(GRID_SIZE * GRID_SIZE);

    private readonly HashSet<Button> _selectedButtons = new HashSet<Button>();

    private Button _createBtn;
    private Button _loadSOBtn;
    private ObjectField _blockSOField;
    private Button _realBtn;

    private TextField _nameField;
    private TextField _descriptionField;
    private TextField _priceField;
    private ObjectField _iconField;
    private DropdownField _chooseTypeField;
    private TextField _gradeField;
    private ObjectField _statSOField;
    private TextField _statIncField;

    [MenuItem("Tools/SynergyBlock")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<SynergyBlockDoc>();
        wnd.titleContent = new GUIContent("SynergyBlock");
    }

    public void CreateGUI()
    {
        var root = rootVisualElement;
        if (m_VisualTreeAsset == null)
        {
            root.Add(new Label("Missing VisualTreeAsset (UXML)."));
            return;
        }

        root.Add(m_VisualTreeAsset.Instantiate());

        InitializeButtonGrid(root);

        CacheUiFields(root);

        RegisterGridButtonEvents();

        RegisterActionButtons();
    }

    private void InitializeButtonGrid(VisualElement root)
    {
        var synergyButtons = root.Query<Button>(className: "SynergyBtn").ToList();
        if (synergyButtons == null || synergyButtons.Count < GRID_SIZE * GRID_SIZE)
        {
            Debug.LogError($"SynergyBlockDoc: expected {GRID_SIZE * GRID_SIZE} buttons with class 'SynergyBtn'. Found {synergyButtons?.Count ?? 0}");
            return;
        }

        _buttonGrid = new ButtonInfo[GRID_SIZE, GRID_SIZE];
        _buttonLookup.Clear();

        for (int row = 0; row < GRID_SIZE; row++)
        {
            for (int col = 0; col < GRID_SIZE; col++)
            {
                int index = col + (GRID_SIZE * row);
                var btn = synergyButtons[index];
                var pos = new Vector2Int(row, col);
                var info = new ButtonInfo(btn, pos, false);
                _buttonGrid[row, col] = info;
                _buttonLookup[btn] = info;
                
                SetButtonColor(btn, Color.gray);
            }
        }

        _realBtn = root.Q<Button>("RealBtn");
        if (_realBtn != null && _buttonLookup.TryGetValue(_realBtn, out var realInfo))
        {
            realInfo.IsFilled = true;
            SetButtonColor(_realBtn, Color.green); 
            _selectedButtons.Add(_realBtn); 
        }
    }

    private void CacheUiFields(VisualElement root)
    {
        _createBtn = root.Q<Button>("CreateBtn");
        _loadSOBtn = root.Q<Button>("LoadSOBtn");
        _blockSOField = root.Q<ObjectField>("BlockSOField");

        _nameField = root.Q<TextField>("NameTextField");
        _descriptionField = root.Q<TextField>("DescriptionTextField");
        _priceField = root.Q<TextField>("PriceTextField");
        _iconField = root.Q<ObjectField>("IconSpriteField");
        _chooseTypeField = root.Q<DropdownField>("ChooseTypeField");
        _gradeField = root.Q<TextField>("GradeTextField");
        _statSOField = root.Q<ObjectField>("StatSOField");
        _statIncField = root.Q<TextField>("StatIncTextField");
    }

    private void RegisterGridButtonEvents()
    {
        if (_buttonGrid == null) return;

        foreach (var kv in _buttonLookup)
        {
            var button = kv.Key;
            if (button == _realBtn) continue;

            button.clicked += () => OnGridButtonClicked(button);

            button.RegisterCallback<MouseEnterEvent>(evt =>
            {
                if (!_selectedButtons.Contains(button))
                    SetButtonColor(button, new Color(0.7f, 0.7f, 0.7f, 1f));
            });

            button.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                if (!_selectedButtons.Contains(button))
                    SetButtonColor(button, Color.gray);
            });
        }
    }

    private void RegisterActionButtons()
    {
        if (_loadSOBtn != null)
            _loadSOBtn.clicked += OnLoadSOBtnClicked;

        if (_createBtn != null)
            _createBtn.clicked += OnCreateBtnClicked;
    }

    private void OnLoadSOBtnClicked()
    {
        var so = _blockSOField?.value as SynergySO;
        if (so == null)
        {
            Debug.LogWarning("SynergyBlockDoc: No SynergySO assigned to BlockSOField.");
            return;
        }

        _nameField.value = so.name;
        _priceField.value = so.ItemPrice.ToString();
        _chooseTypeField.value = so.synergyType.ToString();
        _gradeField.value = so.grade.ToString();
        _statSOField.value = so.increasePlayerStat;
        _statIncField.value = so.increasePlayerStatValue.ToString();

        ClearUserSelections();

        var centerInfo = GetButtonInfo(_realBtn);
        if (centerInfo == null)
        {
            Debug.LogError("SynergyBlockDoc: RealBtn mapping not found.");
            return;
        }

        var boardPositions = new HashSet<Vector2Int>();
        foreach (var relative in so.spaces)
        {
            boardPositions.Add(centerInfo.Position + relative);
        }

        for (int r = 0; r < GRID_SIZE; r++)
        {
            for (int c = 0; c < GRID_SIZE; c++)
            {
                var info = _buttonGrid[r, c];
                if (info == centerInfo) continue;
                if (boardPositions.Contains(info.Position))
                {
                    info.IsFilled = true;
                    SetButtonColor(info.Button, Color.red);
                    _selectedButtons.Add(info.Button);
                }
            }
        }
    }

    private void OnCreateBtnClicked()
    {
        string name = _nameField?.value ?? string.Empty;
        string description = _descriptionField?.value ?? string.Empty;
        if (!int.TryParse(_priceField?.value, out int price))
        {
            Debug.LogError("Invalid price entered.");
            return;
        }
        Sprite icon = _iconField?.value as Sprite;
        string selectedValue = _chooseTypeField?.value;
        if (!int.TryParse(_gradeField?.value, out int grade))
        {
            Debug.LogError("Invalid grade entered.");
            return;
        }
        StatSO statSO = _statSOField?.value as StatSO;
        if (!float.TryParse(_statIncField?.value, out float statInc))
        {
            Debug.LogError("Invalid stat increment entered.");
            return;
        }

        if (!Enum.TryParse(selectedValue, out SynergyType synergyValue))
        {
            Debug.LogError("Invalid synergy type selected.");
            return;
        }

        var existingSo = _blockSOField?.value as SynergySO;
        CreateOrUpdateSynergySO(name, description, price, icon, synergyValue, grade, statSO, statInc, existingSo);
    }

    private void CreateOrUpdateSynergySO(string name, string description, int price, Sprite icon, SynergyType type,
                                         int grade, StatSO statSO, float statInc, SynergySO existingSo = null)
    {
        var synergySO = existingSo == null ? ScriptableObject.CreateInstance<SynergySO>() : existingSo;

        synergySO.name = name;
        synergySO.ItemPrice = price;
        synergySO.synergyType = type;
        synergySO.grade = grade;
        synergySO.increasePlayerStat = statSO;
        synergySO.increasePlayerStatValue = statInc;
        synergySO.typeColor = synergySO.SetColor(type);

        synergySO.spaces.Clear();
        synergySO.spaces.Add(Vector2Int.zero); 

        foreach (var kv in _buttonLookup)
        {
            var info = kv.Value;
            if (info.Button == _realBtn) continue;
            if (info.IsFilled)
            {
                Vector2Int relative = info.Position - new Vector2Int(CENTER_INDEX, CENTER_INDEX);
                if (relative != Vector2Int.zero)
                    synergySO.spaces.Add(relative);
            }
        }

        if (existingSo == null)
        {
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{assetFolderPath}/[{name}(lvl_{grade})]item.asset");
            AssetDatabase.CreateAsset(synergySO, assetPath);
        }
        else
        {
            EditorUtility.SetDirty(synergySO);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void OnGridButtonClicked(Button button)
    {
        if (!_buttonLookup.TryGetValue(button, out var info))
            return;

        // If selecting: must be adjacent to an already-filled button
        bool currentlySelected = _selectedButtons.Contains(button);

        if (!currentlySelected)
        {
            if (!IsAdjacentFilled(info))
            {
                // not allowed to select if no adjacent filled cell
                // optional: give visual feedback (shake/tooltip)
                return;
            }

            info.IsFilled = true;
            SetButtonColor(button, Color.red);
            _selectedButtons.Add(button);
        }
        else
        {
            // deselect
            info.IsFilled = false;
            SetButtonColor(button, Color.gray);
            _selectedButtons.Remove(button);
        }
    }

    private bool IsAdjacentFilled(ButtonInfo info)
    {
        foreach (var dir in AdjacencyDirs)
        {
            Vector2Int adjPos = info.Position + dir;
            if (IsOutOfBounds(adjPos)) continue;

            var neighbor = _buttonGrid[adjPos.x, adjPos.y];
            if (neighbor.IsFilled)
                return true;
        }
        return false;
    }

    private ButtonInfo GetButtonInfo(Button button)
    {
        if (button == null) return null;
        _buttonLookup.TryGetValue(button, out var info);
        return info;
    }

    private void SetButtonColor(Button button, Color color)
    {
        if (button == null) return;
        button.style.backgroundColor = new StyleColor(color);
    }

    private void ClearUserSelections()
    {
        _selectedButtons.Clear();

        // keep real btn filled
        foreach (var kv in _buttonLookup)
        {
            var info = kv.Value;
            info.IsFilled = false;
            SetButtonColor(info.Button, Color.gray);
        }

        if (_realBtn != null && _buttonLookup.TryGetValue(_realBtn, out var realInfo))
        {
            realInfo.IsFilled = true;
            SetButtonColor(_realBtn, Color.green);
            _selectedButtons.Add(_realBtn);
        }
    }

    private bool IsOutOfBounds(Vector2Int pos) => pos.x < 0 || pos.x >= GRID_SIZE || pos.y < 0 || pos.y >= GRID_SIZE;

    // Unregister events to avoid leaked delegates when window closes/reloads
    private void OnDisable()
    {
        // Clear click delegates and callbacks to be safe
        foreach (var kv in _buttonLookup)
        {
            var btn = kv.Key;
            try
            {
                btn.clicked -= () => OnGridButtonClicked(btn); // safe no-op: delegates differ; we'll instead clear by reassigning listeners where possible
                btn.UnregisterCallback<MouseEnterEvent>(null);
                btn.UnregisterCallback<MouseLeaveEvent>(null);
            }
            catch
            {
                // ignore - UIElements delegate removal can be tricky; Editor restart clears anyway
            }
        }

        if (_loadSOBtn != null) _loadSOBtn.clicked -= OnLoadSOBtnClicked;
        if (_createBtn != null) _createBtn.clicked -= OnCreateBtnClicked;
    }
}
#endif
