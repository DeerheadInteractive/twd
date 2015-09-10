using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Tower data. Layout for tower data txt file is at bottom.
/// </summary>
public static class TowerData {
    /// <summary>
    /// Holds all tower data. Follows this layout:<br/>
    /// <c>data [towerName] ["cost" | "value"] [attribute] [upgrade_level]</c>
    /// </summary>
    private static Hashtable data;

    public enum ATTRIBUTE {
        DAMAGE,
        RANGE,
        RATE
    }

    public static void init() {
        if (Load(FileManager.loadTextStream(FileManager.TOWER_PREFIX + "data"))) {
            Debug.Log("Loaded tower data successfully.");
        } else {
            Debug.Log("Error loading tower data.");
        }
    }
    
    private static bool Load(Stream inStream) {
        StreamReader reader = new StreamReader(inStream);
        Tokenizer tokenizer = new Tokenizer();
        data = new Hashtable();
        using (reader) {
            while (!reader.EndOfStream) {
                try {
                    tokenizer.resetWithString(reader.ReadLine());
                    string towerName = tokenizer.nextToken();
                    Debug.Log("Reading tower info for: " + towerName);
                    int base_cost = tokenizer.nextInt();
                    Hashtable table = new Hashtable();
                    data[towerName] = table;
                    table["base_cost"] = base_cost;
                    table["cost"] = new Hashtable();
                    table["value"] = new Hashtable();

                    // Damage
                    loadList(table, "value", ATTRIBUTE.DAMAGE, reader.ReadLine());
                    loadList(table, "cost", ATTRIBUTE.DAMAGE, reader.ReadLine());
                    // Range
                    loadList(table, "value", ATTRIBUTE.RANGE, reader.ReadLine());
                    loadList(table, "cost", ATTRIBUTE.RANGE, reader.ReadLine());
                    // Rate
                    loadList(table, "value", ATTRIBUTE.RATE, reader.ReadLine());
                    loadList(table, "cost", ATTRIBUTE.RATE, reader.ReadLine());
                } catch (IOException e) {
                    Debug.Log("Exception caught while loading tower data:");
                    Debug.Log(e);
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Loads a list of data from the next line of the reader into memory.
    /// </summary>
    /// <param name="table">Table for corresponding tower.</param>
    /// <param name="type">Type (ie. "value" or "cost").</param>
    /// <param name="attribute">Attribute.</param>
    /// <param name="reader">Reader.</param>
    private static void loadList(Hashtable table, string type, ATTRIBUTE attribute, string line) {
        Tokenizer tokenizer = new Tokenizer(line);
        ArrayList list = new ArrayList();
        while (tokenizer.hasNext()) {
            list.Add(tokenizer.nextFloat());
        }

        Hashtable inner;
        if (table.Contains(type)){
            inner = (Hashtable)table[type];
        } else{
            inner = new Hashtable();
            table[type] = inner;
        }
        inner[attribute] = list;
    }

    /// <summary>
    /// Gets the value of an attribute for a tower at a given upgrade level.
    /// </summary>
    /// <returns>The cost.</returns>
    /// <param name="towerName">Tower name.</param>
    /// <param name="level">Level of upgrade.</param>
    /// <param name="attribute">Attribute.</param>
    public static int getUpgradeCost(string towerName, int level, ATTRIBUTE attribute) {
        return (int)getInnerData(towerName, "cost", level, attribute);
    }

    /// <summary>
    /// Gets the value of an attribute for a tower at a given upgrade level.
    /// </summary>
    /// <returns>The value.</returns>
    /// <param name="towerName">Tower name.</param>
    /// <param name="level">Level of upgrade.</param>
    /// <param name="attribute">Attribute.</param>
    public static float getUpgradeValue(string towerName, int level, ATTRIBUTE attribute) {
        return getInnerData(towerName, "value", level, attribute);
    }

    /// <summary>
    /// I don't even know WTF kind of "refactor" this was.
    /// </summary>
    /// <returns>The inner data.</returns>
    /// <param name="towerName">Tower name.</param>
    /// <param name="type">Type.</param>
    /// <param name="level">Level.</param>
    /// <param name="attribute">Attribute.</param>
    private static float getInnerData(string towerName, string type, int level, ATTRIBUTE attribute){
        Hashtable table = (Hashtable)data[towerName];
        Hashtable inner = (Hashtable)table[type];
        ArrayList list = (ArrayList)inner[attribute];
        return (float)list[level];
    }

    /// <summary>
    /// Gets the base cost of a tower.
    /// </summary>
    /// <returns>The base cost.</returns>
    /// <param name="towerName">Tower name.</param>
    public static int getBaseCost(string towerName){
        Hashtable table = (Hashtable)data[towerName];
        return (int)table["base_cost"];
    }

    /// <summary>
    /// Whether or not another upgrade is available for the given tower and attribute.
    /// </summary>
    /// <returns><c>true</c>, if another upgrade is avaliable, <c>false</c> otherwise.</returns>
    /// <param name="towerName">Tower name.</param>
    /// <param name="attribute">Attribute.</param>
    /// <param name="level">Level.</param>
    public static bool canUpgrade(string towerName, ATTRIBUTE attribute, int level){
        Hashtable table = (Hashtable)data[towerName];
        Hashtable inner = (Hashtable)table["cost"];
        ArrayList list = (ArrayList)inner[attribute];
        return list.Count > level;
    }

    public static ATTRIBUTE indexToAttribute(int index){
        switch (index){
            case 0:
                return ATTRIBUTE.DAMAGE;
            case 1:
                return ATTRIBUTE.RANGE;
            case 2:
                return ATTRIBUTE.RATE;
            default:
                return ATTRIBUTE.DAMAGE;
        }
    }

    /// <summary>
    /// Translates an attribute to an index.
    /// For example, the index of DAMAGE is 0. Thus, the tower's damage upgrade level is stored in upgradeLevel[0].
    /// </summary>
    /// <returns>The attribute index.</returns>
    /// <param name="attribute">Attribute.</param>
    public static int attributeToIndex(TowerData.ATTRIBUTE attribute){
        switch (attribute){
            case TowerData.ATTRIBUTE.DAMAGE:
                return 0;
            case TowerData.ATTRIBUTE.RANGE:
                return 1;
            case TowerData.ATTRIBUTE.RATE:
                return 2;
            default:
                return 0;
        }
    }

    /*
     * data.txt layout:
     * 
     * tower_name base_cost    | (No spaces in tower name!)
     * a b c ...  | Damage values
     * i j k ...  | Damage upgrade costs
     * a b c ...  | Range values
     * i j k ...  | Range upgrade costs
     * a b c ...  | Rate values
     * i j k ...  | Rate upgrade costs
     * 
     * tower name (no spaces!)
     * etc.
     * 
     * (eof)
     * 
     * Notes:
     * The first value is the tower's base value for that attribute.
     * The first cost is the cost of the first upgrade for that tower and that attribute.
     * There should be exactly one less cost than value. (eg. 4 values, 3 costs).
     * THERE MUST BE AT LEAST ONE UPGRADE PER ATTRIBUTE PER TOWER. Failure to list at least one number in each row
     * will result in possible death and dismemberment.
     * All costs must be integers.
     * All values will be treated as floats, though these may be cast to integers in Gunnery or its subclasses.
     * 
     * 
     * Example:
     * 
     * SingleTower 50
     * 100 115 140
     * 20 40
     * 5 6 7
     * 30 70
     * 1.0 0.9 0.8
     * 40 70
     */
}
