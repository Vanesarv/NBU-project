namespace NBU.Forum.Infrastructure.Prediction;

using Domain.Prediction;
using Application.Prediction;
using Microsoft.Extensions.ML;

internal sealed class PredictionService : IPredictionService
{
    private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

    public PredictionService(PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        => _predictionEnginePool = predictionEnginePool;

    public bool IsContentToxic(string content)
    {
        var input = new ModelInput { Sentence = content };

        var prediction = _predictionEnginePool.Predict(input);

        bool result = Convert.ToBoolean(prediction.PredictedLabel);

        return !result;
    }
}
