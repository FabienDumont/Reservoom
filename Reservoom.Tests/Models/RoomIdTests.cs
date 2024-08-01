using Reservoom.Models;

namespace Reservoom.Tests.Models;

public class RoomIdTests {
	[Test]
	public void ToString_ReturnsUniqueRoomId()
	{
		// arrange
		RoomId roomId = new(2, 15);

		// act
		string roomIdString = roomId.ToString();

		// assert
		Assert.That(roomIdString, Is.EqualTo("2_15"));
	}
}