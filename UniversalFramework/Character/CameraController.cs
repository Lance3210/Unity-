using UnityEngine;

public class CameraController : MonoBehaviour
{
	//相机旋转：点击左键，三种方式，八方(FPS)，左右，上下
	public enum RotationType
	{
		MouseXAndMouseY,
		MouseX,
		MouseY
	}

	[SerializeField]
	RotationType axes = RotationType.MouseXAndMouseY;//声明旋转方式
	[SerializeField]
	[Range(1, 10)]
	float sensitivity = 10;//灵敏度
	[SerializeField]
	float rotationY = 0;//Y轴角度
	[SerializeField]
	float minAngle = -80, maxAngle = 90;//限制旋转角度最小值和最大值
	public bool isInverse = false;//反转控制
	void Update()
	{
		CameraRotate(axes);
	}

	private void CameraRotate(RotationType rot)
	{
		//八方，FPS
		if (rot == RotationType.MouseXAndMouseY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
			rotationY += Input.GetAxis("Mouse Y") * sensitivity;
			rotationY = Mathf.Clamp(rotationY, minAngle, maxAngle);//限制Y轴角度
			transform.localEulerAngles = new Vector3((isInverse ? 1 : -1) * rotationY, rotationX, 0);
		}

		//左右
		if (rot == RotationType.MouseX)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
			transform.localEulerAngles = new Vector3(0, (isInverse ? -1 : 1) * rotationX, 0);
		}

		//上下
		if (rot == RotationType.MouseY)
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivity;
			rotationY = Mathf.Clamp(rotationY, minAngle, maxAngle);//限制Y轴角度           
			transform.localEulerAngles = new Vector3((isInverse ? 1 : -1) * rotationY, 0, 0);
		}
	}
}

