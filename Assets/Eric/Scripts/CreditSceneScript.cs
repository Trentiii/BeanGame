using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CreditSceneScript : MonoBehaviour
{
    public enum Slide
    {
        Nothing,
        Trent,
        Eric,
        Evan,
        Logan,
        Taya,
        Adam,
        Kadence,
        Olivia
    }
    public Slide CurrentSlide;
    public List<GameObject> Panels;

    // Start is called before the first frame update
    void Start()
    {
        CurrentSlide = Slide.Nothing;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            nextSlide();
        if (CurrentSlide == Slide.Olivia + 1)
            CurrentSlide = Slide.Trent;

        switch (CurrentSlide)
        {
            case Slide.Trent:
                deactivatAllPanels();
                Panels[0].SetActive(true);
                break;
            case Slide.Eric:
                deactivatAllPanels();
                Panels[1].SetActive(true);
                break;
            case Slide.Evan:
                deactivatAllPanels();
                Panels[2].SetActive(true);
                break;
            case Slide.Logan:
                deactivatAllPanels();
                Panels[3].SetActive(true);
                break;
            case Slide.Taya:
                deactivatAllPanels();
                Panels[4].SetActive(true);
                break;
            case Slide.Adam:
                deactivatAllPanels();
                Panels[5].SetActive(true);
                break;
            case Slide.Kadence:
                deactivatAllPanels();
                Panels[6].SetActive(true);
                break;
            case Slide.Olivia:
                deactivatAllPanels();
                Panels[7].SetActive(true);
                break;
            default:
                break;
        }
    }

    private void nextSlide()
    {
        CurrentSlide += 1;
    }
    private void deactivatAllPanels()
    {
        foreach (GameObject panel in Panels)
            panel.SetActive(false);
    }
    public void goToSlide(GameObject button)
    {
        //get button name to make slide appear
        switch (button.name)
        {
            case "Trent":
                CurrentSlide = Slide.Trent;
                break;
            case "Eric":
                CurrentSlide = Slide.Eric;
                break;
            case "Evan":
                CurrentSlide = Slide.Evan;
                break;
            case "Logan":
                CurrentSlide = Slide.Logan;
                break;
            case "Taya":
                CurrentSlide = Slide.Taya;
                break;
            case "Adam":
                CurrentSlide = Slide.Adam;
                break;
            case "Kadence":
                CurrentSlide = Slide.Kadence;
                break;
            case "Olivia":
                CurrentSlide = Slide.Olivia;
                break;
            default:
                break;
        }
    }
}
