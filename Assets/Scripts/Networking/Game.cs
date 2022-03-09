using System.Collections;
using System.Collections.Generic;

public class Game
{
    public string id { get; set; }
    public string size { get; set; }
    public int limit { get; set; }
    public List<Players> clients  { get; set; }
}
