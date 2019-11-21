using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderDisplay : MonoBehaviour {

    public Text text;
    Slider display;

    string message;

    void Start()
    {
        display = transform.GetComponentInParent<Slider>();
        message = text.text;
    }

    void Update()
    {
        text.text = message + display.value;
    }
}
