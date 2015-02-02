import java.math.BigInteger;


public class SignValues {
	private BigInteger s;
	private BigInteger r;
	
	public BigInteger getS() {
		return this.s;
	}
	
	public BigInteger getR(){
		return this.r;
	}
	
	public SignValues(BigInteger s, BigInteger r) {
		this.s = s; this.r = r;
	}
}
