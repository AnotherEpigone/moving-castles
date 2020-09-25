namespace MovingCastles.Maps
{
    public interface IMapFactory
    {
        MovingCastlesMap Create(int width, int height);
    }
}