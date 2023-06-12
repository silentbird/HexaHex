using System;

namespace Game {
	public enum CellDir {
		[OppositeDirection(RightBottom)]
		LeftTop,

		[OppositeDirection(LeftBottom)]
		RightTop,

		[OppositeDirection(Left)]
		Right,

		[OppositeDirection(LeftTop)]
		RightBottom,

		[OppositeDirection(RightTop)]
		LeftBottom,

		[OppositeDirection(Right)]
		Left
	}


	[AttributeUsage(AttributeTargets.Field)]
	public class OppositeDirectionAttribute : Attribute {
		public OppositeDirectionAttribute(CellDir oppositeDirection) {
			this.oppositeDirection = oppositeDirection;
		}

		public CellDir oppositeDirection { get; }
	}
}