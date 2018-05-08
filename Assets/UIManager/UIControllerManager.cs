using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerManager : Singleton<UIControllerManager> {
    BaseUIController[] controllers = null;

    public bool Init()
    {
        return InitControllers();
    }

    public bool InitControllers()
    {
        controllers = new BaseUIController[]
        {
            //new LoginUIController()
        };
        for (int i = 0; i < controllers.Length; i++)
        {
            if (!RegisterController(controllers[i]))
            {
                return false;
            }
        }
        return true;
    }

    bool RegisterController(BaseUIController controller)
    {
        if (!controller.Init())
        {
            Debug.LogWarning("controller init fail : " + controller.ToString());
            return false;
        }
        else
        {
            UIManager.Instance.RegisteController(controller);
            return true;
        }
    }

    public void Dispose()
    {
        DisposeControllers();
    }

    public void DisposeControllers()
    {
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] != null)
            {
                controllers[i].Dispose();
                controllers[i] = null;
            }
        }
        controllers = null;
    }

}
