using JusticeFramework.Components;
using UnityEngine;

namespace JusticeFramework.Logic {
    public delegate void OnPostExecute();

    public abstract class Action {
        public event OnPostExecute onPostExecute;

        protected WorldObject target;
        protected AudioClip sound;
        protected EAudioType audioType = EAudioType.SoundEffect;
        protected float volume = 1;

        public WorldObject Target {
            get { return target; }
            protected set { target = value; }
        }

        public AudioClip Sound {
            get { return sound; }
            set { sound = value; }
        }

        public EAudioType AudioType {
            get { return audioType; }
            set { audioType = value; }
        }

        public float Volume {
            get { return volume; }
            set { volume = value; }
        }

        public virtual bool IsEmptyAction {
            get { return false; }
        }

        public Action(WorldObject target) : this(target, null, EAudioType.SoundEffect, 1) {
        }

        public Action(WorldObject target, AudioClip sound) : this(target, sound, EAudioType.SoundEffect, 1) {
        }

        public Action(WorldObject target, AudioClip sound, EAudioType audioType, float volume) {
            this.target = target;
            this.sound = sound;
            this.audioType = audioType;
            this.volume = volume;
        }

        public void SetSound(AudioClip sound, EAudioType audioType, float volume) {
            this.sound = sound;
            this.audioType = audioType;
            this.volume = volume;
        }

        public void Execute(WorldObject actor) {
            // Play the activation sound
            actor.PlaySound(sound, EAudioType.SoundEffect, volume);

            OnExecute(actor);
            onPostExecute?.Invoke();
        }

        protected abstract void OnExecute(WorldObject actor);
    }
}
