using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace SmartFeedback.Scripts.DataAnalysis;

public class Preprocessing
{
    public string Preprocess(string text)
    {
        ScriptEngine engine = 

        // Получение пути к папке, где находится ваш py-файл
        string pythonScriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Scripts\PythonScripts");

        // Добавление пути к папке, где находится ваш py-файл
        engine.Execute("import sys");
        engine.Execute($"sys.path.append('{pythonScriptsPath}')");

        // Создание объекта для доступа к функции из вашего py-файла
        var scope = engine.CreateScope();
        engine.ExecuteFile(Path.Combine(pythonScriptsPath, "preprocessing_module.py"), scope);

        // Вызов функции preprocess_text
        dynamic preprocessFunction = scope.GetVariable("preprocess_text");
        string result = preprocessFunction(text);

        return result;
    }
}