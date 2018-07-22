public class ResourceSet {

    private static readonly int MAX_RESOURCE_AMOUT = 500;

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

    private static readonly int RESOURCE_PER_SECOND = 25;
    public void addFrom(ResourceSet other)
    {
        // oxygen
        if (other.oxygen < RESOURCE_PER_SECOND)
        {
            oxygen += other.oxygen;
            other.oxygen = 0;
        }
        else
        {
            oxygen += RESOURCE_PER_SECOND;
            other.oxygen -= RESOURCE_PER_SECOND;
        }
        if (oxygen > MAX_RESOURCE_AMOUT) oxygen = MAX_RESOURCE_AMOUT;

        // food
        if (other.food < RESOURCE_PER_SECOND)
        {
            food += other.food;
            other.food = 0;
        }
        else
        {
            food += RESOURCE_PER_SECOND;
            other.food -= RESOURCE_PER_SECOND;
        }
        if (food > MAX_RESOURCE_AMOUT) food = MAX_RESOURCE_AMOUT;

        // fuel
        if (other.fuel < RESOURCE_PER_SECOND)
        {
            fuel += other.fuel;
            other.fuel = 0;
        }
        else
        {
            fuel += RESOURCE_PER_SECOND;
            other.fuel -= RESOURCE_PER_SECOND;
        }
        if (fuel > MAX_RESOURCE_AMOUT) fuel = MAX_RESOURCE_AMOUT;

        // materials
        if (other.materials < RESOURCE_PER_SECOND)
        {
            materials += other.materials;
            other.materials = 0;
        } else
        {
            materials += RESOURCE_PER_SECOND;
            other.materials -= RESOURCE_PER_SECOND;
        }
        if (materials > MAX_RESOURCE_AMOUT) materials = MAX_RESOURCE_AMOUT;
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
