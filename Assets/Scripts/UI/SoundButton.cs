using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoundButton
{
    public class SoundButton : Button
    {
        private bool pointerWasUp;

        private SoundButton buttonSounds;

        protected override void Awake()
        {
            base.Awake();
            buttonSounds = GetComponent<SoundButton>();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (buttonSounds != null)
            {
                SoundManager.GetInstance().PlayUISE(SoundManager.UISEType.Click);
            }

            base.OnPointerClick(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            pointerWasUp = true;
            base.OnPointerUp(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (pointerWasUp)
            {
                pointerWasUp = false;
                base.OnPointerEnter(eventData);
            }
            else
            {
                if (buttonSounds != null)
                {
                    SoundManager.GetInstance().PlayUISE(SoundManager.UISEType.MoveOn);
                }

                base.OnPointerEnter(eventData);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            pointerWasUp = false;
            base.OnPointerExit(eventData);
        }
    }
}