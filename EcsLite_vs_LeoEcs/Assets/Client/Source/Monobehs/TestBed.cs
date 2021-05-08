using System;
using System.Diagnostics;
using Client.Source.Components;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Source.Monobehs
{
    public class TestBed : MonoBehaviour
    {
        [SerializeField] 
        private bool withStopwatch = true;
        [SerializeField] 
        private bool withPrewarm = true;
        [SerializeField] 
        private int defaultNumEntities;
        [SerializeField] 
        private int defaultNumFrames;

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

            if (withStopwatch)
                _stopwatch = new Stopwatch();

            _testLiteBtn = testEcsLiteBtnGO.GetComponent<Button>();
            _testLiteBtn.onClick.AddListener(_onTestLiteClick);
            _resultLiteTxt = resultEcsLiteTxtGO.GetComponent<TextMeshProUGUI>();
            
            _testLeoBtn = testLeoEcsBtnGO.GetComponent<Button>();
            _testLeoBtn.onClick.AddListener(_onTestLeoClick);
            _resultLeoTxt = resultLeoEcsTxtGO.GetComponent<TextMeshProUGUI>();
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
            Destroy(gameObject.GetComponent<TestLite>());
            
            _resultLiteTxt.text = $"MS: {timeResult}";
            EnableUi(true);
        }

        private void _onTestLiteClick()
        {
            var testLite = gameObject.AddComponent<TestLite>();
            testLite.InjectTestBed(this);
            testLite.InjectStopwatch(_stopwatch);

            var entityId = testLite.World.NewEntity();
            if (withPrewarm) testLite.World.GetPool<PrewarmComponent>().Add(entityId);
            _StartTesting(ref testLite.World.GetPool<TestStartEvent>().Add(entityId));
        }

        private void _onTestLeoClick()
        {
            var testLeo = gameObject.AddComponent<TestLeo>();
            testLeo.Inject(this);
            testLeo.Inject(_stopwatch);

            var entity = testLeo.World.NewEntity();
            if (withPrewarm) entity.Get<PrewarmComponent>();
            _StartTesting(ref entity.Get<TestStartEvent>());
        }
        
        public void EndLeoTest(long timeResult)
        {
            Destroy(gameObject.GetComponent<TestLeo>());
            
            _resultLeoTxt.text = $"MS: {timeResult}";
            EnableUi(true);
        }

        private void EnableUi(bool value)
        {
            _testLiteBtn.interactable = value;
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