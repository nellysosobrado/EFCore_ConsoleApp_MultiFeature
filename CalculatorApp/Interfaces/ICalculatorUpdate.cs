using ClassLibrary.Enums.CalculatorAppEnums;

public interface ICalculatorUpdate
{
    Dictionary<string, double> GetSelectedInputsToUpdate(Dictionary<string, double> currentInputs);
    (Dictionary<string, double> parameters, string newOperator) GetSelectedParametersToUpdate(Dictionary<string, double> currentParameters);
    void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator);
    void UpdateCalculation(int id, double operand1, double operand2, CalculatorOperator calculatorOperator, double result);
    Calculator GetUpdatedCalculationValues(int id);
    void ProcessAndSaveCalculation(Calculator calc);
    void DisplayResults(Calculator calc);
}
