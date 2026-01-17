using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using TMPro;

namespace SpinTheBottle
{
    public class SpinTheBottleGame : UdonSharpBehaviour
    {
        
        [UdonSynced, FieldChangeCallback(nameof(SpinBottleCallback))] public byte SpinBottle;
        
        private Transform root;
        private VRCPlayerApi playerLocal;
        private VRCObjectSync objectSync;
        private Animator anim;
        private int hashSpin;

        public TextMeshPro[] textMeshes;
        public TextMeshPro SelectedText;
        public TextMeshPro lastSpunBy;
        public string[] textPrompts;

        [UdonSynced]
        public int[] currentOptions = new int[8];

        [UdonSynced]
        public float TimeToShowTxt;

        public byte SpinBottleCallback
        {
            get => SpinBottle;
            set
            {
                SpinBottle = value;
                anim.SetTrigger(hashSpin);
            }
        }
        
        private void Start()
        {
            SelectedText.text = "";
            lastSpunBy.text = "";

            TimeToShowTxt = 0;
            playerLocal = Networking.LocalPlayer;
            root = transform.parent;
            objectSync = (VRCObjectSync)root.GetComponent(typeof(VRCObjectSync));
            anim = root.GetComponent<Animator>();
            hashSpin = Animator.StringToHash("Spin");

            if (playerLocal.isMaster) // setup initial options
            {
                for (int i = 0; i < textMeshes.Length; i++)
                {
                    SetOptionToRandom(i);
                }
            }
            else
            { // load options if already initilized
                LoadOptions();
            }
            RequestSerialization();
        }
        
        public override void Interact()
        {
            SendCustomEventDelayedSeconds("UpdateText", 2, VRC.Udon.Common.Enums.EventTiming.Update);
            RandomizedSelectedText();

            BottleInteractSpin();
            RequestSerialization();
        }

        public void UpdateText()
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetHoverText");
        }


        private void LoadOptions()
        {
            for (int i = 0; i < textMeshes.Length; i++)
            {
                textMeshes[i].text = textPrompts[currentOptions[i]];
            }

        }

        private void SetOptionToRandom(int optionIndex)
        {
            int option = Random.Range(0, textPrompts.Length);
            while (System.Array.IndexOf(currentOptions, option) != -1 )
            {
                option = Random.Range(0, textPrompts.Length);
            }
            textMeshes[optionIndex].text = textPrompts[option];
            currentOptions[optionIndex] = option;
        }

        private int GetSelectedOption()
        {
            float currentY = root.rotation.eulerAngles.y;
            Debug.Log($"CurrentY {root.rotation.eulerAngles.y}");
            int currentSlot = (int)(currentY / 45);

            return currentSlot;
        }

        private void RandomizedSelectedText()
        {
            int currentSlot = GetSelectedOption();

            Debug.Log($"currentslot {currentSlot}");

            SetOptionToRandom(currentSlot);
        }

        private void BottleInteractSpin()
        {
            //change the text the bottle is pointing at

            //Bottle spin and sync logic
            Networking.SetOwner(playerLocal, gameObject);
            Networking.SetOwner(playerLocal, root.gameObject);
            root.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 359f), 0);
            objectSync.FlagDiscontinuity();
            SpinBottleCallback++;
            if (SpinBottleCallback > 250) SpinBottleCallback = 0;


            RequestSerialization();

            SetHoverText();
        }

        public void SetHoverText()
        {
            int currentSlot = GetSelectedOption();
            SelectedText.text = textPrompts[currentOptions[currentSlot]];
            lastSpunBy.text = $"Spun by: {Networking.GetOwner(gameObject).displayName}";
        }

        public override void OnDeserialization()
        {
            LoadOptions();
            SetHoverText();
        }
    }
}