using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RoomGenerator : BaseLevelGenerator
{
    //TODO - fcole - Allow custom bounds here?
    [System.Serializable]
    public struct RoomGenerationParams
    {
        public LevelGenMap.TileType FillTileType;
        public int NumberOfRooms;
        public int RoomMinSize;
        public int RoomMaxSize;
        public int MaxRetries;
    }

	public LevelGenMap.TileType FillTileType = LevelGenMap.TileType.B;
	public int NumberOfRooms = 3;
	public int RoomMinSize = 4;
	public int RoomMaxSize = 6;
	public List<Rect> Rooms;
	public int MaxRetries = 20;

	void Reset()
	{
		this.GeneratorName = "Room Generator";
	}

    public void ApplyParams(RoomGenerationParams generationParams)
    {
        this.FillTileType = generationParams.FillTileType;
        this.NumberOfRooms = generationParams.NumberOfRooms;
        this.RoomMinSize = generationParams.RoomMinSize;
        this.RoomMaxSize = generationParams.RoomMaxSize;
        this.MaxRetries = generationParams.MaxRetries;
        this.Rooms = new List<Rect>();
    }

	public override void SetupGeneration()
	{
		base.SetupGeneration();
		this.Rooms.Clear();
		_retries = 0;
		this.AddPhase(this.AddRoomsPhase);
	}

    public override LevelGenOutput GetOutput()
    {
        LevelGenOutput output = base.GetOutput();
        output.AddMapInfo(new LevelGenRoomInfo(SimpleRect.RectListToSimpleRectList(this.Rooms)));
        return output;
    }

	/**
	 * Phases
	 */
	public void AddRoomsPhase(int frames)
	{
		int room = this.CurrentPhase.FramesElapsed;

		int finalRoomForStep = room + frames;
		if (finalRoomForStep > this.NumberOfRooms)
			finalRoomForStep = this.NumberOfRooms;

		while (room < finalRoomForStep && _retries < this.MaxRetries)
		{
			int w = Random.Range(this.RoomMinSize, this.RoomMaxSize + 1);
			int h = Random.Range(this.RoomMinSize, this.RoomMaxSize + 1);
			int x = Random.Range(0, this.Map.Width - w);
			int y = Random.Range(0, this.Map.Height - h);
			Rect newRoom = new Rect(x, y, w, h);
			bool failed = false;

			foreach (Rect otherRoom in this.Rooms)
			{
				if (newRoom.Intersects(otherRoom))
				{
					failed = true;
					++_retries;
					break;
				}
			}

			if (!failed)
			{
				createRoom(newRoom);

				if (this.Rooms.Count > 0) // First room has no previous room to connect to
				{
					Vector2 prevRoomCenter = this.Rooms[this.Rooms.Count - 1].center;
					if (Random.Range(0, 2) == 1)
					{
						createHTunnel((int)prevRoomCenter.x, (int)newRoom.center.x, (int)prevRoomCenter.y);
						createVTunnel((int)prevRoomCenter.y, (int)newRoom.center.y, (int)newRoom.center.x);
					}
					else
					{
						createVTunnel((int)prevRoomCenter.y, (int)newRoom.center.y, (int)newRoom.center.x);
						createHTunnel((int)prevRoomCenter.x, (int)newRoom.center.x, (int)prevRoomCenter.y);
					}
				}

				this.Rooms.Add(newRoom);
				++room;
			}
		}

		if (finalRoomForStep == this.NumberOfRooms)
			this.NextPhase();
	}

	/**
	 * Public
	 */

	// Can be used to place player
	public Rect FirstRoom()
	{
		return this.Rooms[0];
	}

	// Can be used to place stairs
	public Rect LastRoom()
	{
		return this.Rooms[this.Rooms.Count - 1];
	}

	// Can be used to populate rooms with items, monsters etc
	public List<Rect> AllRooms()
	{
		return this.Rooms;
	}

	/**
	 * Private
	 */
	private int _retries;

	private void createHTunnel(int x1, int x2, int y)
	{
		for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); ++x)
		{
			this.Map.Grid[x, y] = this.FillTileType;
		}
	}

	private void createVTunnel(int y1, int y2, int x)
	{
		for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); ++y)
		{
			this.Map.Grid[x, y] = this.FillTileType;
		}
	}

	private void createRoom(Rect room)
	{
		this.Map.FillRect(room, this.FillTileType);
	}
}
