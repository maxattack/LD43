using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    //public float MaxOffBalance = 100f;
    public float endDistance = 33000f;

    public RectTransform RatioPanel;
    public RectTransform FireImage;
    public RectTransform MassImage;

    public Text MassVal;
    public Text ThrustVal;

    public RectTransform RouteImage;
    public RectTransform PlayerShipImage;

    public Text TextDestinationDist;
    public Text TextDangerDist;
    public Text PlayerSpeed;

    private float currentPlayerDist = 1000;
    private float currentDangerDist = 0;

    public float PlayerSpeedMultiplier;
    public float DangerSpeedRate;
    public float DangerRandomRange = 5f;

    public GameObject VictoryPanel;
    public GameObject DefeatPanel;
    public Text VictoryBounty;
    public Text DefeatBounty;

    public RectTransform SpeedometerDial;
    public Text SpeedometerText;
    public float MaxSpeed = 120f;

    private void LateUpdate()
    {
        //This is ripped from StatsText
        var mass = ShipMass.MassScale * Ship.inst.GetTotalMass();
        //var totalMass = Mathf.RoundToInt(mass);
        var thrust = 10000 * Ship.inst.GetThrustCount();
        var com = Ship.inst.centerOfMass;

        var bp = com.magnitude / Ship.inst.maxBalancePenaltyMeters;

        var speed = mass > Mathf.Epsilon ?
            Mathf.RoundToInt((1f - bp) * thrust / mass) : 0f;

        PlayerSpeed.text = "SPEED:\n" + speed + " au/s";
        SpeedometerText.text = speed.ToString();
        float dialRot = Mathf.InverseLerp(0,MaxSpeed, speed);
        SpeedometerDial.rotation = Quaternion.Euler(0,0, Mathf.Lerp(-300,-95,1-dialRot));

        UpdateRatio(mass, thrust, bp, com.x);

        currentPlayerDist += speed * Time.deltaTime * PlayerSpeedMultiplier;
        currentDangerDist += Time.deltaTime * (DangerSpeedRate + Random.Range(-DangerRandomRange, DangerRandomRange));

        UpdateDistances(currentPlayerDist, currentDangerDist);

    }

    public void UpdateRatio(float mass, float thrust, float balancePenalty, float roll) {

        float y = Mathf.Lerp(-30f, 30f, 1f - (mass * 10)/thrust);
        RatioPanel.anchoredPosition = new Vector2(0, y);

        float rot = Mathf.Lerp( 0, 45, balancePenalty * 2);
        if (roll > 0) rot *= -1;
        FireImage.rotation = Quaternion.Euler(0, 0, rot);
        MassImage.rotation = Quaternion.Euler(0, 0, rot);

        MassVal.text = Mathf.FloorToInt(mass).ToString();
        ThrustVal.text = Mathf.FloorToInt(thrust).ToString();
    }

    public void UpdateDistances(float playerDist, float dangerDist) {

        TextDestinationDist.text = "Destination:\n" + Mathf.FloorToInt(endDistance - playerDist).ToString() + " au";
        TextDangerDist.text = "DANGER:\n" + Mathf.FloorToInt(playerDist - dangerDist).ToString() + " au";

        //Shrink the entire route as danger approaches end goal
        float dangerY = Mathf.InverseLerp(0, endDistance, dangerDist);
        RouteImage.anchorMin = new Vector2(0.5f, dangerY);
        //Do the same method with the player's ship
        float playerY = Mathf.InverseLerp(0, endDistance, playerDist);
        PlayerShipImage.anchorMin = new Vector2(0.5f, playerY);

        if(playerDist < dangerDist) {
            Defeat();
        }
        if(playerDist >= endDistance) {
            Victory();
        }
    }

    void Defeat() {
        DefeatBounty.text = "Died clutching $" + Ship.inst.GetTotalBooty().ToString();
        DefeatPanel.SetActive(true);
    }

    void Victory() {
        VictoryBounty.text = "Scored: " + Ship.inst.GetTotalBooty().ToString();
        VictoryPanel.SetActive(true);
    }
}
