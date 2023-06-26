namespace NBU.Forum.Domain.Prediction;

using Microsoft.ML.Data;

public class ModelOutput
{
    [ColumnName(@"sentence")]
    public float[] Sentence { get; set; } = default!;

    [ColumnName(@"score")]
    public uint Score { get; set; }

    [ColumnName(@"Features")]
    public float[] Features { get; set; } = default!;

    [ColumnName(@"PredictedLabel")]
    public float PredictedLabel { get; set; }

    [ColumnName(@"Score")]
    public float[] PredictionScore { get; set; } = default!;
}
