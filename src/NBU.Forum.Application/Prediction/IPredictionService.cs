namespace NBU.Forum.Application.Prediction;

public interface IPredictionService
{
    bool IsContentToxic(string content);
}
