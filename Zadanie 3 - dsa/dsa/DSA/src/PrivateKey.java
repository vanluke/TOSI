import java.math.BigInteger;


public class PrivateKey {
	private BigInteger p;
	private BigInteger q;
	private BigInteger y;
	private BigInteger g;
	
	public BigInteger getP(){
		return this.p;
	}
	public BigInteger getQ(){
		return this.q;
	}
	public BigInteger getY(){
		return this.y;
	}
	public BigInteger getG(){
		return this.g;
	}
	
	/**
	 * 
	 * @param p p
	 * @param q q
	 * @param g g
	 * @param y y
	 */
	public PrivateKey(BigInteger p, BigInteger q, BigInteger g, BigInteger y) {
		this.p = p; this.q =q; this.g = g; this.y= y;
	}
}
