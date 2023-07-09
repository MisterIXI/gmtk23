using UnityEngine;
using static UnityEngine.Random;

public class StreamerAnimator : MonoBehaviour
{
    [field: SerializeField] private Animator _anim;
    [field: SerializeField] private MeshRenderer[] _eyes;
    [field: SerializeField] private MeshRenderer[] _eyebrows;
    [field: SerializeField] private MeshRenderer _mouth;
    [field: SerializeField] private Material _eye_normal;
    [field: SerializeField] private Material _eye_surprised;
    [field: SerializeField] private Material _eye_straight;
    [field: SerializeField] private Material _eyebrows_normal;
    [field: SerializeField] private Material _eyebrows_high;
    [field: SerializeField] private Material _eyebrows_angry;
    [field: SerializeField] private Material _mouth_aah;
    [field: SerializeField] private Material _mouth_frown;
    [field: SerializeField] private Material _mouth_happy;
    [field: SerializeField] private Material _mouth_neutral;
    [field: SerializeField] private Material _mouth_open;
    [field: SerializeField] private Material _mouth_smirk;
    private float _tMouth;
    private float _tEyebrow;
    private float _tEye;
    private float _tAnim;
    private GameSettings _settings;
    private void Awake()
    {
        SubscribeEvents();
        _settings = SettingsHolder.Instance.GameSettings;
    }

    private void FixedUpdate()
    {
        if (_tEye > Time.time)
        {
            _tEye = Time.time + _settings.EyeCD * Random.value;
            int randomInt = Random.Range(0, 4);
            Material mat = _eye_normal;
            switch (randomInt)
            {
                case 0:
                    mat = _eye_normal;
                    break;
                case 1:
                    mat = _eye_straight;
                    break;
                case 2:
                    mat = _eye_surprised;
                    break;
            }
            foreach (var eye in _eyes)
            {
                eye.material = mat;
            }
        }
        if (_tMouth > Time.time)
        {
            _tMouth = Time.time + _settings.MouthCD * Random.value;
            int randomInt = Random.Range(0, 4);
            Material mat = _mouth_aah;
            switch (randomInt)
            {
                case 0:
                    mat = _mouth_aah;
                    break;
                case 1:
                    mat = _mouth_frown;
                    break;
                case 2:
                    mat = _mouth_happy;
                    break;
                case 3:
                    mat = _mouth_neutral;
                    break;
                case 4:
                    mat = _mouth_open;
                    break;
                case 5:
                    mat = _mouth_smirk;
                    break;
            }
            _mouth.material = mat;
        }
        if (_tEyebrow > Time.time)
        {
            _tEyebrow = Time.time + _settings.EyebrowCD * Random.value;
            int randomInt = Random.Range(0, 4);
            Material mat = _eyebrows_normal;
            switch (randomInt)
            {
                case 0:
                    mat = _eyebrows_normal;
                    break;
                case 1:
                    mat = _eyebrows_high;
                    break;
                case 2:
                    mat = _eyebrows_angry;
                    break;
            }
            foreach (var eyebrow in _eyebrows)
            {
                eyebrow.material = mat;
            }
        }
    }
    private void SubscribeEvents()
    {

    }

    private void UnSubscribeEvents()
    {

    }
    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
}