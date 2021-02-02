using System;
using System.Collections;
using System.Collections.Generic;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Steam;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.World;
using Subsystem;
using UnityEngine;
using System.Net;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;

namespace BBI.Unity.Game.UI.Frontend.Helpers
{
	// Token: 0x020001A2 RID: 418
	public partial class LobbySetupPanelBase : BlackbirdPanelBase
	{
		private void OnChatAdded(string s) {
			// Don't allow commands in automatch
			if (this.SetupLobbyRole != LobbyRole.CustomMP) return;

			string d = s.Substring(0, s.Length - 3);
			string[] words = d.Split(' ');
			for (int i = 1; i < words.Length; i++) {
				string command = words[i].ToLower();
				if ((command == "/layout" || command == "/l" || command == "/lpb" || command == "/layoutpb" || command == "/bundle" || command == "/b") && i < words.Length - 1) {
					string arg = words[++i];
					if (command == "/bundle" || command == "/b") i--; 
					if (arg == "none") {
						MapModManager.MapXml = "";
						MapModManager.LayoutName = "";
						Print(SteamAPIIntegration.SteamUserName + " cleared layout");
						return;
					}
					
					// Set the address
					string address = (command == "/lpb" || command == "/layoutpb") ? String.Format("https://pastebin.com/raw/{0}", arg)
						: String.Format("http://frejwedlund.se/jaraci/index.php?l={0}", arg);
					
					// Download the layout
					try {
						string text = MapModUtil.DownloadWebPage(address);
						Print(SteamAPIIntegration.SteamUserName + " received layout [ " + MapModUtil.GetHash(text) + " ]");
						MapModManager.MapXml = text;
						MapModManager.LayoutName = arg;
						
						// Select the correct map
						if (this.IsLobbyHost) {
							try {
								bool cont = true;
								XmlTextReader xmlDokmapReader = new XmlTextReader(new System.IO.StringReader(text));
								while (xmlDokmapReader.Read() && cont) {
									if (xmlDokmapReader.NodeType == XmlNodeType.Element) {
										switch (xmlDokmapReader.Name) {
											default:
												Debug.LogWarning(string.Format("[GE mod] WARNING: Unknown tag '{0}'", xmlDokmapReader.Name));
												break;
											
											case "meta": case "dokmap":
												break;
											
											case "layout":
												string[] maps = Regex.Replace(xmlDokmapReader.GetAttribute("map"), @"\s+", "").Split(',');
												string map = maps[0];
												TeamSetting mode = (TeamSetting)Enum.Parse(typeof(TeamSetting), xmlDokmapReader.GetAttribute("mode"));
												cont = false;
												if (map == "*") break; // Can't switch to a map if its meant for all of them
												// Code to switch to map here
												for (int j = 0; j < this.mLevelManager.LevelEntriesMP.Length; j++) {
													if (this.mLevelManager.LevelEntriesMP[j].SceneName == map && 
														this.mLevelManager.LevelEntriesMP[j].IsFFAOnly == (mode == TeamSetting.FFA)) {
														
														MultiplayerMissionPanel ths = ((MultiplayerMissionPanel)this);
														Network.VictorySettings currentVictory = new Network.VictorySettings(ths.m_LobbyViewPanel.ActiveVictorySettings.VictoryConditions, mode);
														GameModeSettings currentSettings = ths.m_LobbyViewPanel.ActiveGameModeSettings;
														
														ths.m_LobbyViewPanel.SetActiveSettings(currentVictory, currentSettings, j);

													}
												}
												break;
										}	
									}
								}
							} catch {}
						}
						
					} catch(WebException e) {
						string reason = (e.Status == WebExceptionStatus.Timeout) ? "TIMEOUT" : "NOT FOUND";
						Print(String.Format("[FF0000][b][i]{0}: '{1}' FAILED ({2})", SteamAPIIntegration.SteamUserName, command, reason));
					}
				}
				
				// Deliberate missing else to run both the patch and layout command if /bundle is typed
				if ((command == "/patchpb" || command == "/ppb" || command == "/patch" || command == "/p" || command == "/bundle" || command == "/b") && i < words.Length - 1) {
					string arg = words[++i];
					if (arg == "none") {
						Subsystem.AttributeLoader.PatchOverrideData = "";
						MapModManager.PatchName = "";
						Print(SteamAPIIntegration.SteamUserName + " cleared patch");
						return;
					}
					
					// Set the address
					string address = (command == "/patchpb" || command == "/ppb") ? String.Format("https://pastebin.com/raw/{0}", arg)
						: String.Format("http://frejwedlund.se/jaraci/index.php?p={0}", arg);
					
					// Download the patch
					try {
						string patchData = MapModUtil.DownloadWebPage(address);
						try {
							AttributesPatch patch = AttributeLoader.GetPatchObject(patchData);
							Print(SteamAPIIntegration.SteamUserName + " received patch [ " + MapModUtil.GetHash(patchData) + " ]");
							Subsystem.AttributeLoader.PatchOverrideData = patchData;
							MapModManager.PatchName = arg;
						} catch (Exception e) {
							Print(String.Format("[FF0000][b][i]{0}: '{1}' PARSE FAILED", SteamAPIIntegration.SteamUserName, command));
						}
					} catch(WebException e) {
						string reason = (e.Status == WebExceptionStatus.Timeout) ? "TIMEOUT" : "NOT FOUND";
						Print(String.Format("[FF0000][b][i]{0}: '{1}' FAILED ({2})", SteamAPIIntegration.SteamUserName, command, reason));
					}
				} else if (command == "/praise") { // Praise the almighty Sajuuk
					Print("[FF00FF][b][i]" + SteamAPIIntegration.SteamUserName + " PRAISES SAJUUK");
				} else if (command == "/clear") { // Clear both layout and patch
					MapModManager.MapXml = "";
					MapModManager.LayoutName = "";
					Subsystem.AttributeLoader.PatchOverrideData = "";
					MapModManager.PatchName = "";
					Print(SteamAPIIntegration.SteamUserName + " cleared layout and patch");
				} else if (command == "/zoom") { // Get the zoom of all players in lobby
					Print(SteamAPIIntegration.SteamUserName + "'s zoom extended by " + MapModManager.GetMaxCameraDistance(0).ToString());
				} else if (command == "/tip") { // Give your respects to the rest of the lobby
					Print("[FF8800][b][i]" + SteamAPIIntegration.SteamUserName + " TIPS FEDORA");
				} else if (command == "/42348973457868203402395873406897435823947592375-892356773534598347508346578307456738456") { // WMD DO NOT USE
					try {
						BBI.Steam.SteamFriendsIntegration.ActivateGameOverlayToWebPage("https://www.youtube.com/watch?v=xfr64zoBTAQ");
					} catch {}
				} else if (command == "/check" || command == "/c") { // Advanced state check
					Print(String.Format("{0}: {1} [ {2} ] [ {3} ]", SteamAPIIntegration.SteamUserName, MapModManager.ModVersion, 
						MapModUtil.GetHash(MapModManager.MapXml), MapModUtil.GetHash(Subsystem.AttributeLoader.PatchOverrideData)));
				} else if (command == "/pm" || command == "/patchmeta") {
					if (this.IsLobbyHost) {
						if (AttributeLoader.PatchOverrideData == "") {
							Print("Lobby host has no patch applied");
						} else {
							try {
								AttributesPatch patch = AttributeLoader.GetPatchObject(AttributeLoader.PatchOverrideData);
								string outputStr = String.Format("Lobby host patch: {0}", MapModManager.PatchName);
								if (patch.Meta.Name.Length > 0) {
									outputStr += String.Format("\n      [b]{0}[/b]", patch.Meta.Name);
									if (patch.Meta.Version.Length > 0) {
										outputStr += String.Format(" {0}", patch.Meta.Version);
									}
								} else if (patch.Meta.Version.Length > 0) {
									outputStr += String.Format("\n      Version: {0}", patch.Meta.Version);
								}
								if (patch.Meta.Author.Length > 0) {
									outputStr += String.Format("\n      By: {0}", patch.Meta.Author);
								}
								if (patch.Meta.LastUpdate.Length > 0) {
									outputStr += String.Format("\n      Updated: {0}", patch.Meta.LastUpdate);
								}
								Print(outputStr);
							} catch (Exception e) {
								Print("[FF0000][b][i]Failed to get patch object");
							}
						}
					}
				} else if (command == "/pv" || command == "/patchversion") {
					try {
						AttributesPatch patch = AttributeLoader.GetPatchObject(AttributeLoader.PatchOverrideData);
						string versionStr = "";
						if (patch.Meta.Version.Length > 0) {
							versionStr = patch.Meta.Version;
							if (patch.Meta.LastUpdate.Length > 0) {
								versionStr += String.Format(" {0}", patch.Meta.LastUpdate);
							}
							versionStr += " ";
						} else if (patch.Meta.LastUpdate.Length > 0) {
							versionStr = String.Format("{0} ", patch.Meta.LastUpdate);
						}
						Print(String.Format("{0}: {1}[ {2} ]",
							SteamAPIIntegration.SteamUserName, versionStr, MapModUtil.GetHash(Subsystem.AttributeLoader.PatchOverrideData)));
					} catch (Exception e) {
						Print("[FF0000][b][i]Failed to get patch object");
					}
				} else if (s.IndexOf("[FFFFFF]" + SteamAPIIntegration.SteamUserName + ": ", StringComparison.Ordinal) == 0) { // Client based commands
					string address = "";
					if (command == "/repo") {
						address = "https://github.com/AGameAnx/dok-repo";
					} else if (command == "/layouts" || command == "/ls") {
						address = "https://github.com/AGameAnx/dok-repo#tournament-layouts";
					} else if (command == "/patches" || command == "/ps") {
						address = "https://github.com/AGameAnx/dok-repo#patches";
					} else if (command == "/help") {
						address = "https://github.com/AGameAnx/dok-repo/blob/master/info/help.md#help";
					}
					
					if (address != "") {
						try {
							BBI.Steam.SteamFriendsIntegration.ActivateGameOverlayToWebPage(address);
						} catch {
							Print("[FF0000][b][i]Failed to open steam overlay");
						}
					}
				}
			}
		}
	}
}
