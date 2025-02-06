using UnityEngine;

namespace AudioSystem
{
    public class SoundBuilder : MonoBehaviour
    {
        readonly SoundManager _soundManager;
        SoundData _soundData;
        Vector3 _position = Vector3.zero;
        bool _randomPitch;

        public SoundBuilder(SoundManager soundManager)
        {
            this._soundManager = soundManager;
        }

        public SoundBuilder WithSoundData(SoundData soundData)
        {
            this._soundData = soundData;
            return this;
        }

        public SoundBuilder WithPosition(Vector3 position)
        {
            this._position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch()
        {
            this._randomPitch = true;
            return this;
        }

        public void Play()
        {
            if (!_soundManager.CanPlaySound(_soundData))
                return;

            SoundEmitter soundEmitter = _soundManager.Get();
            soundEmitter.Initialize(_soundData);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = SoundManager.Instance.transform;

            if (_randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            if (_soundData.FrequentSound)
            {
                _soundManager.FrequentSoundEmitters.Enqueue(soundEmitter);
            }

            soundEmitter.Play();
        }
    }
}