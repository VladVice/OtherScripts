using System.Collections.Generic;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Oxide.Core.Plugins;
using Oxide.Core;
using Oxide.Core.Configuration;
using System.Linq;
using Oxide.Game.Rust.Cui;

namespace Oxide.Plugins
{
	[Info("TagReward", "weedburger", "0.0.1")]
    [Description("Dog Tag Reward for kill")]
    public class TagReward : RustPlugin
    {
		int TagCount;
		int DogTag = 1223900335; // Refers to dog tag item ID
		

		/*[ChatCommand("top")]
		void Top(BasePlayer player)
		{
			//string[] files = Directory.GetFiles(@"C:\Server_Data\rustserver\oxide\data\Player_Tags");
			//Interface.Oxide.DataFileSystem.ReadObject<string>("Player_Tags/");


			var Path = Interface.Oxide.DataFileSystem.GetFile("Player_Tags/");
			
			SendReply(player, "[GameWorld]: {0} ", Path);
			
			/*foreach(string file in Path.GetFiles("*.json"))
			{
				SendReply(player, "[GameWorld]: {0} ", file);
			}
		}*/
		
		[ChatCommand("tags")]
		void Tags(BasePlayer player)
		{
			string playerID = player.userID.ToString();
			
			try
			{
				string tags = Interface.Oxide.DataFileSystem.ReadObject<string>("Player_Tags/" + player.userID.ToString());
				SendReply(player, "[GameWorld]: You have collected {0} tags.", tags);

			}
			catch(Exception E)
			{
				SendReply(player, "[GameWorld]: You have to deposit some Dog Tags first.");
			}
		}
			
		[ChatCommand("deposit")]
		void Deposit(BasePlayer player)
		{
			if(player.inventory.GetAmount(DogTag) > 0)
			{
				int TagCount = player.inventory.GetAmount(DogTag); // Get ammount
				
				player.inventory.Take(null, DogTag, TagCount);  // Remove dog tag item from inv
				SendReply(player, "[GameWorld]: Deposited {0} tags.", TagCount);
				
				try
				{
					string tags = Interface.Oxide.DataFileSystem.ReadObject<string>("Player_Tags/" + player.userID.ToString());
					int depoTags = Int32.Parse(tags);
					TagCount = TagCount + depoTags;
				}
				catch(Exception E)
				{
					
				}
				
				Interface.Oxide.DataFileSystem.WriteObject("Player_Tags/" + player.userID.ToString(), TagCount);
			}
			else
			{
				SendReply(player, "[GameWorld]: No Dog Tags to deposit.");
			}
		}
		
        private void OnEntityDeath(BasePlayer player, HitInfo info)
        {
			// parbauda vai tas ir ir speletais
            var initiator = info?.InitiatorPlayer;
			
            if (initiator == null || player.userID.IsSteamId() == false || initiator?.userID.IsSteamId() == false || player == initiator)
            {
                return;
            }
			
			Item itemtogive = ItemManager.CreateByItemID(ItemManager.FindItemDefinition(DogTag).itemid, 1);
			
			//if( IfPlayer Alive >= 30min)
			itemtogive.Drop(player.GetDropPosition(), player.GetDropVelocity());
        }
    }
	
	
}