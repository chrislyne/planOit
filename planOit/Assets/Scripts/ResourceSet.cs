public class ResourceSet {

    public int food;
    public int oxygen;
    public int fuel;
    public int materials;

    public bool ResourceDepleted
    {
        get { return food <= 0 || oxygen <= 0 || fuel <= 0; }
    }

    public int ResourceTotal
    {
        get {  return food + oxygen + fuel + materials; }
    }

    public ResourceSet(int fuel, int oxygen, int food, int materials)
    {
        this.fuel = fuel;
        this.oxygen = oxygen;
        this.food = food;
        this.materials = materials;
    }

    public override string ToString()
    {
        return "Food:" + food + " Oxygen:" + oxygen + " Fuel:"+ fuel + " Materials:" + materials;
    }
}
