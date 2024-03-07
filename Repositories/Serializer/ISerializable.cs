namespace SOSTeam.TravelAgency.Repositories.Serializer
{
    public interface ISerializable
    {
        string[] ToCSV();
        void FromCSV(string[] values);

    }
}
