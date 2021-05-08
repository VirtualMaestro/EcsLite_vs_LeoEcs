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
        private TestLite _testLite;
        private Stopwatch _stopwatch;
        
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

            _stopwatch = new Stopwatch();

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
            testBed.Timer = _stopwatch;
        }

        private void _PrepareLeo()
        {
            _testLeoBtn = testLeoEcsBtnGO.GetComponent<Button>();
            _testLeoBtn.onClick.AddListener(_onTestLeoClick);
            _resultLeoTxt = resultLeoEcsTxtGO.GetComponent<TextMeshProUGUI>();
        }

        public void WriteLiteLog(string log)
        {
            _resultLiteTxt.text = log;
        }

        public void WriteLeoLog(string log)
        {
            _resultLeoTxt.text = log;
        }

        public void EndLiteTest(long timeResult)
        {
            _resultLiteTxt.text = $"MS: {timeResult}";
            _testLite.enabled = false;
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
            var testLeo = gameObject.AddComponent<TestLeo>();
            testLeo.Inject(this);
            testLeo.Inject(_stopwatch);
            _leoWorld = testLeo.World;
            
            _StartTesting(ref EcsEntityExtensions.Get<TestStartEvent>(_leoWorld.NewEntity()));
        }
        
        public void EndLeoTest(long timeResult)
        {
            Destroy(gameObject.GetComponent<TestLeo>());
            
            _leoWorld = null;
            _resultLeoTxt.text = $"MS: {timeResult}";
            EnableUi(true);
        }

        private void EnableUi(bool value)
        {
            _testLiteBtn.interactable = _isLiteEnabled ? value : _isLiteEnabled;
            _testLeoBtn.interactable = value;
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