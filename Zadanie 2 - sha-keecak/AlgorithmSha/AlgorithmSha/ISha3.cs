using AlgorithmSha.Helpers;

namespace AlgorithmSha
{
    public interface ISha3
    {
        string Create(string M, Options options);
    }
}
