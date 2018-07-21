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

    

    public int getResourceByIndex(int index) {
        switch(index) {
            case 0: return oxygen;
            case 1: return food;
            case 2: return fuel;
            case 3: return materials;
            default: return 0;
        }
    }

    public override string ToString()
    {
        return "Food:" + food + " Oxygen:" + oxygen + " Fuel:"+ fuel + " Materials:" + materials;
    }
}
