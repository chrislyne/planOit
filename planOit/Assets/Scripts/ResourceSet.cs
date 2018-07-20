﻿public class ResourceSet {

    public int food;
    public int oxygen;
    public int fuel;
    public int materials;

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

}
