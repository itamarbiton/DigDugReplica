using UnityEngine;

public class ProceduralSecondOrderDynamics : MonoBehaviour
{
    [SerializeField] private float verticalFrequency = 1f; // the frequency of the vertical dynamics
    [SerializeField] private float verticalDampingRatio = 0.7f; // the damping ratio of the vertical dynamics
    [SerializeField] private float verticalPositionGain = 1f; // the gain applied to the vertical position input

    [SerializeField] private float horizontalFrequency = 1f; // the frequency of the horizontal dynamics
    [SerializeField] private float horizontalDampingRatio = 0.7f; // the damping ratio of the horizontal dynamics
    [SerializeField] private float horizontalPositionGain = 1f; // the gain applied to the horizontal position input

    private SecondOrderDynamics verticalDynamics;
    private SecondOrderDynamics horizontalDynamics;

    private float targetVerticalPosition = 0f;
    private float targetHorizontalPosition = 0f;

    [SerializeField] Transform target;

    void Start()
    {
        UpdateDynamics();
    }

    void Update()
    {
        // update the target positions based on input
        float verticalInput = Input.GetAxisRaw("Vertical");
        targetVerticalPosition = target.position.y;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        targetHorizontalPosition = target.position.x;

        // update the dynamics with the current position and velocity inputs
        float currentVerticalVelocity = (targetVerticalPosition - transform.position.y);
        float currentVerticalPosition = verticalDynamics.Update(Time.deltaTime, transform.position.y, currentVerticalVelocity);

        float currentHorizontalVelocity = (targetHorizontalPosition - transform.position.x);
        float currentHorizontalPosition = horizontalDynamics.Update(Time.deltaTime, transform.position.x, currentHorizontalVelocity);

        // update the position of the game object based on the dynamics output
        transform.position = new Vector3(currentHorizontalPosition, currentVerticalPosition, transform.position.z);
    }

    void UpdateDynamics()
    {
        // initialize the vertical dynamics with the current vertical parameter values and an initial position of 0
        verticalDynamics = new SecondOrderDynamics(verticalFrequency, verticalDampingRatio, verticalPositionGain, 0f);

        // initialize the horizontal dynamics with the current horizontal parameter values and an initial position of 0
        horizontalDynamics = new SecondOrderDynamics(horizontalFrequency, horizontalDampingRatio, horizontalPositionGain, 0f);
    }

    void OnValidate()
    {
        UpdateDynamics();
    }
}

public class SecondOrderDynamics
{
	private float xp; // previous input 
	private float y, yd; // state variables 
	private float k1, k2, k3; // dynamics constants

	private float PI = Mathf.PI;

	public SecondOrderDynamics(float f, float z, float r, float x0)
	{
		// compute constants
		k1 = z / (PI * f);
		k2 = 1 / ((2 * PI * f) * (2 * PI * f));
		k3 = r * z / (2 * PI * f);

		// initialize variables
		xp = x0;
		y = x0;
		yd = 0f;
	}

	public float Update(float T, float x, float? xd = null)
	{
		if (xd == null)
		{ // estimate velocity
			xd = (x - xp) / T;
			xp = x;
		}

		y = y + T * yd; // integrate position by velocity
		yd = (float)(yd + T * (x + k3 * xd - y - k1 * yd) / k2); // integrate velocity by acceleration

		return y;
	}
}