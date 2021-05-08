using System;
using System.Diagnostics;
using Client.Source.Components;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EcsEntityExtensions = Leopotam.Ecs.EcsEntityExtensions;

namespace Client.Source.Monobehs
{
    public class TestBed : MonoBehaviour
    {
        [SerializeField] 
        private int defaultNumEntities;
        [SerializeField] 
        private int defaultNumFrames;

        [SerializeField] 
        private GameObject testLeoGO;
        [SerializeField] 
        private GameObject testLiteGO;
        
        [SerializeField]
        private GameObject testEcsLiteBtnGO;
        [SerializeField]
        private GameObject resultEcsLiteTxtGO;

        [SerializeField]
        private GameObject testLeoEcsBtnGO;
        [SerializeField]
        private GameObject resultLeoEcsTxtGO;

        [SerializeField]
        private GameObject inputNumEntitiesGO;
        [SerializeField]
        private GameObject delayDestroyGO;

        [SerializeField]
        private GameObject deviceSpecGO;

        private Button _testLiteBtn;
        private Button _testLeoBtn;
        private TextMeshProUGUI _resultLiteTxt;
        private TextMeshProUGUI _resultLeoTxt;
        private TMP_InputField _numEntitiesInput;
        private TMP_InputField _delayDestroyInput;
        private TextMeshProUGUI _deviceSpecTxt;
        
        private EcsWorld _liteWorld;
        private Leopotam.Ecs.EcsWorld _leoWorld;
        private int _testBedEntityId;

        private bool _isLiteEnabled;
        private bool _isLeoEnabled;
        private TestLeo _testLeo;
        private TestLite _testLite;
        
        private void Start()
        {
            _numEntitiesInput = inputNumEntitiesGO.GetComponent<TMP_InputField>();
            _numEntitiesInput.contentType = TMP_InputField.ContentType.IntegerNumber;
            _numEntitiesInput.characterLimit = 10;
            
            _delayDestroyInput = delayDestroyGO.GetComponent<TMP_InputField>();
            _delayDestroyInput.contentType = TMP_InputField.ContentType.DecimalNumber;
            _delayDestroyInput.characterLimit = 5;
            
            _deviceSpecTxt = deviceSpecGO.GetComponent<TextMeshProUGUI>();
            _deviceSpecTxt.text = _GetDeviceInfo();

            _PrepareLite();
            _PrepareLeo();
        }

        private string _GetDeviceInfo()
        {
            var description = SystemInfo.deviceType switch
            {
                DeviceType.Desktop => "PC",
                DeviceType.Handheld => "Mobile",
                DeviceType.Console => "Console",
                _ => string.Empty
            };

            description += $": {SystemInfo.operatingSystem}, \n Device: {SystemInfo.deviceModel}";
            return description;
        }

        private void _PrepareLite()
        {
            _isLiteEnabled = testLiteGO.activeSelf;
            _testLiteBtn = testEcsLiteBtnGO.GetComponent<Button>();
            _testLiteBtn.interactable = _isLiteEnabled;
            
            if (!_isLiteEnabled) return;

            _testLite = testLiteGO.GetComponent<TestLite>(); 
            _liteWorld = _testLite.World;
            _testLite.enabled = false;
            
            _testLiteBtn.onClick.AddListener(_onTestLiteClick);
            _resultLiteTxt = resultEcsLiteTxtGO.GetComponent<TextMeshProUGUI>();
            
            _testBedEntityId = _liteWorld.NewEntity();
            ref var testBed = ref _liteWorld.GetPool<TestBedComponent>().Add(_testBedEntityId);
            testBed.TestBed = this;
            testBed.Timer = new Stopwatch();
        }

        private void _PrepareLeo()
        {
            _isLeoEnabled = testLeoGO.activeSelf;
            _testLeoBtn = testLeoEcsBtnGO.GetComponent<Button>();
            _testLeoBtn.interactable = _isLeoEnabled;
            
            if (!_isLeoEnabled) return;

            _testLeo = testLeoGO.GetComponent<TestLeo>(); 
            _leoWorld = _testLeo.World;
            _testLeo.enabled = false;
            
            _testLeoBtn.onClick.AddListener(_onTestLeoClick);
            _resultLeoTxt = resultLeoEcsTxtGO.GetComponent<TextMeshProUGUI>();
            
            ref var testBed = ref EcsEntityExtensions.Get<TestBedComponent>(_leoWorld.NewEntity());
            testBed.TestBed = this;
            testBed.Timer = new Stopwatch();
        }

        public void WriteLiteLog(string log)
        {
            _resultLiteTxt.text = log;
        }

        public void WriteLeoLog(string log)
        {
            _resultLeoTxt.text = log;
        }

        public void SetLiteResult(long milliseconds)
        {
            _resultLiteTxt.text = $"MS: {milliseconds}";
            _testLite.enabled = false;
            EnableUi(true);
        }

        public void SetLeoResult(long milliseconds)
        {
            _resultLeoTxt.text = $"MS: {milliseconds}";
            _testLeo.enabled = false;
            EnableUi(true);
        }

        private void _onTestLiteClick()
        {
            _testLite.enabled = true;
            ref var startEvent = ref _liteWorld.GetPool<TestStartEvent>().Add(_liteWorld.NewEntity());

            _StartTesting(ref startEvent);
        }

        private void _onTestLeoClick()
        {
            _testLeo.enabled = true;
            ref var startEvent = ref EcsEntityExtensions.Get<TestStartEvent>(_leoWorld.NewEntity());
            
            _StartTesting(ref startEvent);
        }

        private void EnableUi(bool value)
        {
            _testLiteBtn.interactable = _isLiteEnabled ? value : _isLiteEnabled;
            _testLeoBtn.interactable = _isLeoEnabled ? value : _isLeoEnabled;
            _numEntitiesInput.interactable = value;
            _delayDestroyInput.interactable = value;
        }

        private void _StartTesting(ref TestStartEvent startEvent)
        {
            startEvent.NumEntities = string.IsNullOrEmpty(_numEntitiesInput.text) ? defaultNumEntities : Math.Abs(int.Parse(_numEntitiesInput.text));
            startEvent.DelayDestroy = string.IsNullOrEmpty(_delayDestroyInput.text) ? defaultNumFrames : Math.Abs(int.Parse(_delayDestroyInput.text));
            
            EnableUi(false);
        }
    }
}