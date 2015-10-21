using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class LevelDataRow
{
    [XmlIgnoreAttribute]
    public int RowLength { get { return this.RowData.Length; } }
    public string RowData = "";
}

[XmlRoot("LevelData")]
[System.Serializable]
public class LevelData
{
    [XmlIgnoreAttribute]
    public int[,] Grid;

    public const char WALL = 'X';

    /**
     * XML
     */
    [XmlArray("Rows"), XmlArrayItem("Row")]
    public LevelDataRow[] Rows;

    public static LevelData Load(string path)
    {
        LevelData level = null;
        var serializer = new XmlSerializer(typeof(LevelData));

        using (var stream = new FileStream(path, FileMode.Open))
        {
            level = serializer.Deserialize(stream) as LevelData;
        }

        int yLength = level.Rows.Length;
        int xLength = yLength > 0 ? level.Rows[0].RowLength : 0;

        level.Grid = new int[xLength, yLength];

        for (int y = 0; y < yLength; ++y)
        {
            char[] row = level.Rows[y].RowData.ToCharArray();

            for (int x = 0; x < xLength; ++x)
            {
                level.Grid[x, y] = row[x] == WALL ? 1 : 0;
            }
        }

        return level;
    }
}
