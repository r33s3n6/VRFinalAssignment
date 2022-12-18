# README

## Map



<img src="README.assets\image-20221218211206547.png" alt="image-20221218211206547" style="zoom:67%;" />

| 编号 | 位置            | 3D模型下坐标  | 相机朝向（Rotate-y） |
| ---- | --------------- | ------------- | -------------------- |
| 0    | bedroom_corner  | 10, 1, -1.2   | 135                  |
| 1    | bedroom_door    | 6.5, 1, -1.5  | 225                  |
| 2    | toilet2_door    | 8, 1, 0       | 270                  |
| 3    | toilet2         | 5.9, 1, 0     | 270                  |
| 4    | bedroom_windows | 10.7, 1, -3.5 | 210                  |
| 5    | center2         | 2.4, 1, -1.5  | 90                   |
| 6    | center          | 0.75, 1, -1   | 135                  |
| 7    | main_door       | 0, 1, 0.6     | 270                  |
| 8    | kitchen         | 2.35, 1, 0.6  | 90                   |
| 9    | toilet          | -3.4, 1, 0.6  | 270                  |
| 10   | ktv             | -2, 1, -1     | 180                  |

起始于位置0，每个位置对应Skybox材质位于Resources文件夹下同名Material，具体图片位于Skybox/位置名下

**TODO：7-8 7-9 7-10三条边的Mobius切换**

## Scene&Objects

- `SampleScene`(0)：全景图场景
  - `Canvas`—`Canvas[i]`—`Canvas[i]_[j]`—`Button_to_[k]`：位置 **i** 的第 **j** 个Canvas下到位置 **k** 的按钮
  - `SceneData`：场景参数传递
  - `Transition Controller`：挂载同名脚本，漫游总控制

- `3DModelScene`(1)：3D模型场景

## Scripts

### Transition Controller

- `public Material[] skyboxMaterial`：每个位置Skybox材质
- `public GameObject[] canvas`：每个位置的Canvas
- `public Vector3[] cameraPos, cameraOrient`：每个位置对应相机在3D模型下位置及朝向
- 以上已于editor中设置
- `void Start()`：设置起始位置、重置朝向，默认0、(0,0,0)，Model切换场景时通过场景1传递参数；淡入后Active对应位置的Canvas以显示移动按钮
- `void Move(int from, int to, int type)`：从位置from移动至位置to，类型type
  - `type=0`：Teleport，切换Skybox，由于相机朝向不一致旋转对齐视角，切换Canvas
  - `type=1`：Model，切换场景，在场景1移动后回到场景0，通过`SceneData`传递参数
  - **TODO** `type=2`：Mobius, 提供from到to的方向向量、二者相机朝向，切换后流程参照Teleport

### Button Click

- `public int from, to, type`：按钮对应起始、目标位置以及切换类型，已于editor中设置

- `void Click()`：调用`transitionController.Move`

### Move Controller

控制`3DModelScene`中摄像机的移动

### Scene Data

Scene之间传递参数
