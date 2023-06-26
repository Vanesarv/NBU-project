namespace NBU.Forum.Domain.Prediction;

using Microsoft.ML.Data;

public class ModelInput
{
    [ColumnName(@"sentence")]
    public string Sentence { get; set; } = default!;

    [ColumnName(@"score")]
    public float Score { get; set; }
}
