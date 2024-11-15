using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
#if DG_IAP
using UnityEngine.Purchasing;
#endif

public static class DGHelper
{
    public static float ParseFloat(string value)
    {
        if (value.Contains(","))
            value = value.Replace(",", ".");
        float tmpFloat = float.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
        return tmpFloat;
    }
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        return;
#else
        Application.Quit();
#endif
    }
    #if DG_IAP
    public static string GetFakePrice(ProductMetadata metadata, float salePercent)//, int round = -1)
    {
        if (metadata == null)
        {
            return "?";
        }

        string _sp = Regex.Replace(metadata.localizedPriceString, @"[\d., ]", string.Empty);
        _sp = metadata.localizedPriceString.Replace(_sp, string.Empty);

        var fakeValue = (float)metadata.localizedPrice / ((100.0f - salePercent) / 100.0f);
        //if (round == -1)
        //{
        if (fakeValue > 10000.0f)
        {
            fakeValue = (float)Math.Round(fakeValue);
            fakeValue = (float)(Math.Round(fakeValue / 1000.0f) * 1000);
        }
        else if (fakeValue > 1000.0f)
        {
            fakeValue = (float)Math.Round(fakeValue);
            fakeValue = (float)(Math.Round(fakeValue / 100.0f) * 100);
        }
        else if (fakeValue > 100.0f)
        {
            fakeValue = (float)Math.Round(fakeValue);
        }
        else
        {
            fakeValue = (float)Math.Round(fakeValue, 2);
        }
        //}
        //else
        //{
        //    fakeValue = (float)Math.Round(fakeValue, round);
        //}
        if (string.IsNullOrEmpty(_sp))
        {
            return "?";
        }
        if (fakeValue * 100 % 100 == 0)
        {
            return metadata.localizedPriceString.Replace(_sp, fakeValue.ToString("0,0", CultureInfo.InvariantCulture));
        }

        return metadata.localizedPriceString.Replace(_sp, fakeValue.ToString("0.00", CultureInfo.InvariantCulture));
    }
#endif
    public static DateTime ParseUnixTimestampNormal(long timestamp)
    {
        return (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(timestamp);
    }
    public static long ToUnixTimestampNormal(this DateTime value)
    {
        return (long)(value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
    public static string TimeToString_2(TimeSpan t)
    {
        if (t.Ticks < 0)
            return string.Format("{0:00}m:{1:00}s", 0, 0);
        if (t.TotalDays >= 1)
        {
            return string.Format("{0:00}d:{1:00}h", t.Days, t.Hours);
        }
        else if (t.TotalHours >= 1)
        {
            return string.Format("{0:00}h:{1:00}m", t.Hours, t.Minutes);
        }
        else
        {
            return string.Format("{0:00}m:{1:00}s", t.Minutes, t.Seconds);
        }
    }
    //public static string TimeToString_2(TimeSpan t)
    //{
    //    if (t.Ticks < 0)
    //        return "0m:0s";
    //    StringBuilder sb = new StringBuilder();
    //    if (t.TotalDays >= 1)
    //    {
    //        sb.Append(t.Days);
    //        sb.Append("d:");
    //        sb.Append(t.Hours);
    //        sb.Append("h");
    //        return sb.ToString();
    //    }
    //    else if (t.TotalHours >= 1)
    //    {
    //        sb.Append(t.Hours);
    //        sb.Append("h:");
    //        sb.Append(t.Minutes);
    //        sb.Append("m");
    //        return sb.ToString();
    //    }
    //    else
    //    {

    //        sb.Append(t.Minutes);
    //        sb.Append("m:");
    //        sb.Append(t.Seconds);
    //        sb.Append("s");
    //        return sb.ToString();
    //    }
    //}
    public static string TimeToString_3(TimeSpan t)
    {
        if (t.Ticks < 0)
            return "0h:0m:0s";
        StringBuilder sb = new StringBuilder();
        if (t.TotalDays >= 1)
        {
            sb.Append(t.Days);
            sb.Append("d:");
            sb.Append(t.Hours);
            sb.Append("h:");
            sb.Append(t.Minutes);
            sb.Append("m");
            return sb.ToString();
        }
        else
        {
            sb.Append(t.Hours);
            sb.Append("h:");
            sb.Append(t.Minutes);
            sb.Append("m:");
            sb.Append(t.Seconds);
            sb.Append("s");
            return sb.ToString();
        }
    }
    public static float GetDistance2(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }
#if UNITY_EDITOR
    public static IEnumerator IELoadData(string urlData, System.Action<string> actionComplete, bool showAlert = false)
    {
        var www = new WWW(urlData);
        float time = 0;
        //TextAsset fileCsvLevel = null;
        while (!www.isDone)
        {
            time += 0.001f;
            if (time > 10000)
            {
                yield return null;
                //Debug.Log("Downloading...");
                time = 0;
            }
        }
        if (!string.IsNullOrEmpty(www.error))
        {
            UnityEditor.EditorUtility.DisplayDialog("Notice", "Load CSV Fail", "OK");
            yield break;
        }
        yield return null;
        actionComplete?.Invoke(www.text);
        yield return null;
        UnityEditor.AssetDatabase.SaveAssets();
        //if (showAlert)
        //    UnityEditor.EditorUtility.DisplayDialog("Notice", "Load Data Success", "OK");
        //else
        Debug.Log("<color=yellow>Download CSV Complete</color>");
    }
#endif
}
