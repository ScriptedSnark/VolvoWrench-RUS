using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Media3D;
using VolvoWrench.Demo_stuff;

namespace PortalAdjust.Demo
{
	internal class Portal2CoopCourse6GameHandler : OrangeBoxGameHandler
	{
		private string _startAdjustType;

		private string _endAdjustType;

        private string[] _maps = Category.Portal2CoopCourse6.Maps;

		private int _startTick = -1;

		private int _endTick = -1;

		private StringBuilder _debugBuffer;

		public Portal2CoopCourse6GameHandler()
		{
			base.Maps.AddRange(this._maps);
			this._debugBuffer = new StringBuilder();
		}

		public override DemoParseResult GetResult()
		{
			DemoParseResult result = base.GetResult();
			if (this._startAdjustType != null)
			{
				result.StartAdjustmentType = this._startAdjustType;
				result.StartAdjustmentTick = this._startTick;
			}
			if (this._endAdjustType != null)
			{
				result.EndAdjustmentType = this._endAdjustType;
				result.EndAdjustmentTick = this._endTick;
			}
			return result;
		}

		protected override ConsoleCmdResult ProcessConsoleCmd(BinaryReader br)
		{
			ConsoleCmdResult consoleCmdResult = base.ProcessConsoleCmd(br);
			StringBuilder stringBuilder = this._debugBuffer;
			object[] currentTick = new object[] { base.CurrentTick, ": ", consoleCmdResult.Command, Environment.NewLine };
			stringBuilder.Append(string.Concat(currentTick));
			if (this._startAdjustType == null && base.CurrentTick > 0 && consoleCmdResult.Command == "ss_force_primary_fullscreen 0")
			{
                this._startAdjustType = "Portal2 co-op course 6 Start Standard";
				this._startTick = base.CurrentTick;
			}
			if (this._endAdjustType == null && base.CurrentTick > 0)
			{
                if (consoleCmdResult.Command.StartsWith("playvideo_end_level_transition") && base.Map != "mp_coop_paint_crazy_box")
				{
					this._endAdjustType = "Portal2 co-op course 6 End Standard";
					this._endTick = base.CurrentTick;
				}
                else if (consoleCmdResult.Command == "playvideo_exitcommand_nointerrupt dlc1_endmovie end_movie movie_outro" && base.Map == "mp_coop_paint_crazy_box")
				{
                    this._endAdjustType = "Portal2 co-op course 6 Run End";
					this._endTick = base.CurrentTick;
				}
			}
			return consoleCmdResult;
		}

		protected override PacketResult ProcessPacket(BinaryReader br)
		{
			PacketResult packetResult = base.ProcessPacket(br);
			StringBuilder stringBuilder = this._debugBuffer;
			object[] currentTick = new object[] { base.CurrentTick, ": ", packetResult.CurrentPosition, Environment.NewLine };
			stringBuilder.Append(string.Concat(currentTick));
			if (this._startAdjustType == null && packetResult.CurrentPosition.Equals(new Point3D(-9896f, -4400f, 3048f)))
			{
				this._startAdjustType = "Portal2 co-op Atlas Gain Control";
				this._startTick = base.CurrentTick;
			}
			else if (packetResult.CurrentPosition.Equals(new Point3D(-11168f, -4384f, 3040.03125f)))
			{
				this._startAdjustType = "Portal2 co-op P-Body Gain Control";
				this._startTick = base.CurrentTick;
			}
			return packetResult;
		}
	}
}