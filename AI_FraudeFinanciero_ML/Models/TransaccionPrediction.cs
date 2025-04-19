using Microsoft.ML.Data;

namespace AI_FraudeFinanciero_ML.Models;

public class TransaccionPrediction
{
    public float Score { get; set; }
    
    public bool PredictedLabel { get; set; }

    public float Probability { get; set; }
}
