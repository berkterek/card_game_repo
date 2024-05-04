using CardGame.Abstracts.DataAccessLayers;
using CardGame.Abstracts.Managers;

namespace CardGame.Managers
{
    public class SaveLoadManager : ISaveLoadService
    {
        readonly IDataSaveLoadDal _dataSaveLoadDal;

        public SaveLoadManager(IDataSaveLoadDal dataSaveLoadDal)
        {
            _dataSaveLoadDal = dataSaveLoadDal;
        }

        public void SaveDataProcess(string key, object value)
        {
            _dataSaveLoadDal.SaveData(key, value);
        }

        public T LoadDataProcess<T>(string key)
        {
            var value = _dataSaveLoadDal.LoadData<T>(key);
            return value;
        }

        public void SaveUnityObjectProcess(string key, UnityEngine.Object value)
        {
            _dataSaveLoadDal.SaveUnityObject(key, value);
        }

        public T LoadUnityObjectProcess<T>(string key) where T : UnityEngine.Object
        {
            var value = _dataSaveLoadDal.LoadUnityObject<T>(key);
            return value;
        }

        public bool HasKeyAvailable(string key)
        {
            return _dataSaveLoadDal.HasKey(key);
        }

        public void DeleteData(string name)
        {
            _dataSaveLoadDal.DeleteData(name);
        }
    }
}