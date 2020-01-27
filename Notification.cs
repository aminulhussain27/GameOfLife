using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using NotificationSamples.iOS;
#endif
using UnityEngine;
using Utilities;

public class NotificationManager : MonoBehaviour
{

    private string channelName = "spongebob_channel";
    private int inactiveUserAlarmTime = 5;//minutes

    public static NotificationManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = new NotificationManager();
        }

        Init();
    }

    private void Init()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        CreateNotificationChannel();
    }

    private void CreateNotificationChannel()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = channelName,
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    public int ShowNotification(string Title, string Description, float Time = 1)
    {
        var notification = new AndroidNotification();
        notification.Title = Title;
        notification.Text = Description;
        notification.SmallIcon = "Icon_Small";
        notification.SmallIcon = "Icon_Large";
        notification.FireTime = DateTime.Now.AddMinutes(Time);

        int identifier = AndroidNotificationCenter.SendNotification(notification, channelName);
        return identifier;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //CancelAllNotification();
            AndroidNotificationCenter.CancelAllNotifications();
        }
        else
        {
            //Schedule all notification
            //TriggerAdNotification();
            TriggerFullLifeNotification();
            TriggerDailyRewardsNotification();
            //TriggerInactiveNotification();
            //TriggerUpgradeNotification();

        }
    }


    void TriggerAdNotification()
    {
        if (string.IsNullOrEmpty(Global.LastFreeGemsCollectTime))
        {
            Debug.LogError("Ad is already active");
            return;
        }

        DateTime lastCollectedTime = DateTime.ParseExact(Global.LastFreeGemsCollectTime, Constants.DateTimeFormat, null);

        DateTime endTime = lastCollectedTime.AddMinutes(LoadDataHandler.rewardAdInfo.cooldown);

        //Debug.LogError(lastCollectedTime + "  ::  " + ServerDateTime.Now + "    : " + (float)(endTime - ServerDateTime.Now).TotalMinutes);
        float totalMinute = (float)(endTime - ServerDateTime.Now).TotalMinutes;

        totalMinute = AddQAValues(totalMinute, 1);

        ShowNotification("Free gems", "Ads are ready to watch", totalMinute);
    }

    void TriggerCookieNotification()
    {
        float totalMinute = (float)LoadDataHandler.CookiesData.Alarm.Span.TotalMinutes;
        //for QA
        totalMinute = AddQAValues(totalMinute);

        if (totalMinute > 0)
        {
            ShowNotification("Cookie", "Cookies are ready to collect", totalMinute);
        }
    }

    void TriggerFullLifeNotification()
    {
        float timeRequireTofullLive = ((Local.MaxLives - Global.AvailableLives) * Local.LifeGenrateMinutes) - (float)LivesSystemHandler.Instance.AlarmInstance.Span.TotalMinutes;
        Debug.LogError("Full life time: " + timeRequireTofullLive);

        //for QA
        timeRequireTofullLive = AddQAValues(timeRequireTofullLive, 2);

        if (Local.MaxLives > Global.AvailableLives && timeRequireTofullLive > 0)
        {
            ShowNotification("Full of Life", "Life is fully refilled", timeRequireTofullLive);
        }
    }

    void TriggerDailyRewardsNotification()
    {
        if (string.IsNullOrEmpty(Global.LastDRewardCollectTime))
        {
            Debug.LogError("Daily rewards is already active");

            return;
        }
        DateTime lastCollection = DateTime.ParseExact(Global.LastDRewardCollectTime, Constants.DateFormat, null);
        DateTime now = DateTime.Now;//ServerDateTime.Now; 

        Debug.LogError("Daily rewards notification : " + (lastCollection.AddHours(24) - now).TotalMinutes);

        if ((float)(lastCollection.AddHours(24) - now).TotalMinutes > 0)
        {
            ShowNotification("Daily Rewards", "daily rewards ready to collect", (float)(lastCollection.AddHours(24) - now).TotalMinutes);
        }
    }

    void TriggerUpgradeNotification()
    {
        UpgradesDeliveryHandler deliveryHandler = UpgradesDeliveryHandler.upgradesDeliveryHandler;
        if (deliveryHandler.DeliverableItemList.Count <= 0)
        {
            Debug.LogError("No items are uder upgrade");
            return;
        }
        float upgradeTimer = (float)deliveryHandler.DeliverableItemList[0].AlarmInstanceObject.Span.TotalMinutes;
        Debug.LogError("Upgrade items:  " + deliveryHandler.DeliverableItemList[0].ItemType + " : " + upgradeTimer);

        for (int i = 0; i < deliveryHandler.DeliverableItemList.Count; i++)
        {
            ShowNotification(deliveryHandler.DeliverableItemList[i].ItemType.ToString(), "Upgrade is ready", upgradeTimer);
        }

    }


    float AddQAValues(float f, float value = 1)
    {
        if (f > 5)
        {
            f = value;
        }
        return f;
    }

    void TriggerInactiveNotification()
    {
        ShowNotification("Hello", "you been not playing for long time", inactiveUserAlarmTime);
    }

}
