﻿using System;
using System.Collections.Generic;
using ShipManifest.InternalObjects;
using ShipManifest.Modules;
using UnityEngine;

namespace ShipManifest.Windows.Tabs.Control
{
  internal static class TabLight
  {
    internal static string ToolTip = "";
    internal static bool ToolTipActive;
    internal static bool ShowToolTips = true;

    internal static void Display(Vector2 displayViewerPosition)
    {
      //float scrollX = WindowControl.Position.x + 20;
      //float scrollY = WindowControl.Position.y + 50 - displayViewerPosition.y;
      float scrollX = 20;
      float scrollY = 50 - displayViewerPosition.y;

      // Reset Tooltip active flag...
      ToolTipActive = false;
      SMHighlighter.IsMouseOver = false;

      GUILayout.BeginVertical();
      GUI.enabled = true;
      //GUILayout.Label("External Light Control Center ", SMStyle.LabelTabHeader);
      GUILayout.Label(SMUtils.Localize("#smloc_control_light_000"), SMStyle.LabelTabHeader);
      GUILayout.Label("____________________________________________________________________________________________",
        SMStyle.LabelStyleHardRule, GUILayout.Height(10), GUILayout.Width(350));
      string step = "start";
      try
      {
        // Display all Lights
        List<ModLight>.Enumerator iLights = SMAddon.SmVessel.Lights.GetEnumerator();
        while (iLights.MoveNext())
        {
          if (iLights.Current == null) continue;
          string label = iLights.Current.Status + " - " + iLights.Current.Title;
          bool onState = iLights.Current.IsOn;
          bool newOnState = GUILayout.Toggle(onState, label, GUILayout.Width(325), GUILayout.Height(40));
          step = "button toggle check";
          if (!onState && newOnState)
            iLights.Current.TurnOnLight();
          else if (onState && !newOnState)
            iLights.Current.TurnOffLight();
          Rect rect = GUILayoutUtility.GetLastRect();
          if (Event.current.type != EventType.Repaint || !rect.Contains(Event.current.mousePosition)) continue;

          SMHighlighter.IsMouseOver = true;
          SMHighlighter.MouseOverRect = new Rect(scrollX + rect.x, scrollY + rect.y, rect.width, rect.height);
          SMHighlighter.MouseOverPart = iLights.Current.SPart;
          SMHighlighter.MouseOverParts = null;
        }
        iLights.Dispose();

        // Display MouseOverHighlighting, if any
        SMHighlighter.MouseOverHighlight();
      }
      catch (Exception ex)
      {
        SMUtils.LogMessage(
          string.Format(" in Light Tab at step {0}.  Error:  {1} \r\n\r\n{2}", step, ex.Message, ex.StackTrace), SMUtils.LogType.Error,
          true);
      }
      GUILayout.EndVertical();
    }

    internal static void TurnOnAllLights()
    {
      // iterate thru the hatch parts and open hatches
      List<ModLight>.Enumerator iLights = SMAddon.SmVessel.Lights.GetEnumerator();
      while (iLights.MoveNext())
      {
        if (iLights.Current == null) continue;
        iLights.Current.TurnOnLight();
      }
      iLights.Dispose();
    }

    internal static void TurnOffAllLights()
    {
      // iterate thru the hatch parts and open hatches
      List<ModLight>.Enumerator iLights = SMAddon.SmVessel.Lights.GetEnumerator();
      while (iLights.MoveNext())
      {
        if (iLights.Current == null) continue;
        iLights.Current.TurnOffLight();
      }
      iLights.Dispose();
    }
  }
}