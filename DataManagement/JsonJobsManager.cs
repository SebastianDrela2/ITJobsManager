﻿using AppliedJobsManager.UI;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace AppliedJobsManager.DataManagement;

internal class JsonJobsManager
{
    private readonly string _jsonAppDataPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\JobsManager\jobs.json";

    public JsonJobsManager()
    {
        CreateDirectoryIfDoesntExist();
    }
    public ObservableCollection<DataItem> LoadJobs()
    {
        if (File.Exists(_jsonAppDataPath))
        {
            var json = File.ReadAllText(_jsonAppDataPath);
            var dataItems = JsonConvert.DeserializeObject<ObservableCollection<DataItem>>(json);

            if (dataItems is not null)
            {
                return dataItems;
            }
        }

        return new ObservableCollection<DataItem>();
    }

    public void SaveJobs(ObservableCollection<DataItem> dataItems)
    {
        var json = JsonConvert.SerializeObject(dataItems);
        File.WriteAllText(_jsonAppDataPath, json);
    }

    private void CreateDirectoryIfDoesntExist()
    {
        var aboveDir = Path.GetDirectoryName(_jsonAppDataPath);

        if (!Directory.Exists(aboveDir))
        {
            Directory.CreateDirectory(aboveDir!);
        }
    }

}