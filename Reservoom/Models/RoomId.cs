namespace Reservoom.Models;

public class RoomId {
	public int FloorNumber { get; }
	public int RoomNumber { get; }

	public RoomId(int floorNumber, int roomNumber) {
		FloorNumber = floorNumber;
		RoomNumber = roomNumber;
	}

	public override string ToString() {
		return $"{FloorNumber}_{RoomNumber}";
	}

	public override bool Equals(object? obj) {
		return obj is RoomId roomId && FloorNumber == roomId.FloorNumber && RoomNumber == roomId.RoomNumber;
	}

	public static bool operator ==(RoomId roomId1, RoomId roomId2) {
		return roomId1.Equals(roomId2);
	}

	public static bool operator !=(RoomId roomId1, RoomId roomId2) {
		return !(roomId1 == roomId2);
	}

	public override int GetHashCode() {
		return HashCode.Combine(FloorNumber, RoomNumber);
	}
}