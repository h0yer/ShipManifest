﻿using System;
using System.Collections.Generic;
using ShipManifest.APIClients;
using ShipManifest.InternalObjects;
using ShipManifest.Modules;
using UnityEngine;

namespace ShipManifest.Windows.Tabs.Control
{
  internal static class TabAntenna
  {
    internal static string ToolTip = "";
    internal static bool ToolTipActive;
    internal static bool ShowToolTips = true;
    internal static bool IsRtAntennas;

    internal static void Display(Vector2 displayViewerPosition)
    {
      //float scrollX = WindowControl.Position.x;
      //float scrollY = WindowControl.Position.y + 50 - displayViewerPosition.y;
      float scrollX = 0;
      float scrollY = 50 - displayViewerPosition.y;

      // Reset Tooltip active flag...
      ToolTipActive = false;
      SMHighlighter.IsMouseOver = false;

      GUILayout.BeginVertical();
      GUI.enabled = true;
      GUILayout.Label(
        //InstalledMods.IsRtInstalled ? "Antenna Control Center  (RemoteTech detected)" : "Antenna Control Center ",
        InstalledMods.IsRtInstalled ? SMUtils.Localize("#smloc_control_antenna_001") : SMUtils.Localize("#smloc_control_antenna_000"),
        SMStyle.LabelTabHeader);
      GUILayout.Label("____________________________________________________________________________________________",
        SMStyle.LabelStyleHardRule, GUILayout.Height(10), GUILayout.Width(350));
      string step = "start";
      try
      {
        // Display all antennas
        List<ModAntenna>.Enumerator iAntennas = SMAddon.SmVessel.Antennas.GetEnumerator();
        while (iAntennas.MoveNext())
        {
          if (iAntennas.Current == null) continue;
          if (!IsRtAntennas && iAntennas.Current.IsRtModule) IsRtAntennas = true;
          step = "get Antenna label";
          string label = iAntennas.Current.AntennaStatus + " - " + iAntennas.Current.Title;
          bool open = iAntennas.Current.Extended;
          bool newOpen = GUILayout.Toggle(open, label, GUILayout.Width(325), GUILayout.Height(40));
          step = "button toggle check";
          if (!open && newOpen)
            iAntennas.Current.ExtendAntenna();
          else if (open && !newOpen)
            iAntennas.Current.RetractAntenna();

          Rect rect = GUILayoutUtility.GetLastRect();
          if (Event.current.type == EventType.Repaint && rect.Contains(Event.current.mousePosition))
          {
            SMHighlighter.IsMouseOver = true;
            SMHighlighter.MouseOverRect = new Rect(scrollX + rect.x, scrollY + rect.y, rect.width, rect.height);
            SMHighlighter.MouseOverPart = iAntennas.Current.SPart;
            SMHighlighter.MouseOverParts = null;
          }
        }
        iAntennas.Dispose();

        // Display MouseOverHighlighting, if any
        SMHighlighter.MouseOverHighlight();
      }
      catch (Exception ex)
      {
        SMUtils.LogMessage(
          string.Format(" in Antenna Tab at step {0}.  Error:  {1} \r\n\r\n{2}", step, ex.Message, ex.StackTrace),
          SMUtils.LogType.Error, true);
      }
      GUILayout.EndVertical();
    }

    internal static void ExtendAllAntennas()
    {
      // TODO: for realism, add a closing/opening sound
      List<ModAntenna>.Enumerator iAntennas = SMAddon.SmVessel.Antennas.GetEnumerator();
      while (iAntennas.MoveNext())
      {
        if (iAntennas.Current == null) continue;
        iAntennas.Current.ExtendAntenna();
      }
      iAntennas.Dispose();
    }

    internal static void RetractAllAntennas()
    {
      // TODO: for realism, add a closing/opening sound
      List<ModAntenna>.Enumerator iAntennas = SMAddon.SmVessel.Antennas.GetEnumerator();
      while (iAntennas.MoveNext())
      {
        if (iAntennas.Current == null) continue;
        iAntennas.Current.RetractAntenna();
      }
      iAntennas.Dispose();
    }
  }
}