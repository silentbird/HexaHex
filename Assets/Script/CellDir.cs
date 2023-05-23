using System;

namespace Script {
	public enum CellDir {
		[OppositeDirectionAttribute(RightBottom)]
		LeftTop,

		[OppositeDirectionAttribute(LeftBottom)]
		RightTop,

		[OppositeDirectionAttribute(Left)]
		Right,

		[OppositeDirectionAttribute(LeftTop)]
		RightBottom,

		[OppositeDirectionAttribute(RightTop)]
		LeftBottom,

		[OppositeDirectionAttribute(Right)]
		Left,
	}


	[AttributeUsage(AttributeTargets.Field)]
	public class OppositeDirectionAttribute : Attribute {
		public CellDir OppositeDirection { get; }

		public OppositeDirectionAttribute(CellDir oppositeDirection) {
			OppositeDirection = oppositeDirection;
		}
	}
}