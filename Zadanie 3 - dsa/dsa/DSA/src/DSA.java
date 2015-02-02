import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Random;

public class DSA {
	int primeCenterie = 20;
	private BigInteger q;
	private BigInteger p;
	private BigInteger r;
	private BigInteger g;
	private BigInteger y;
	private BigInteger s;
	private BigInteger x;
	private BigInteger k;
	private Random rand = new Random();
	private SecurityKeys security;
	
	public SecurityKeys getSecurity() {
		return security;
	}
	
	public DSA() {
	}

	public SecurityKeys generateKey() {
		q = new BigInteger(160, primeCenterie, rand);

		p = generateP(q, 512);
		g = generateG(p, q);
		do {
			x = new BigInteger(q.bitCount(), rand);
		} while (x.compareTo(BigInteger.ZERO) != 1 && x.compareTo(q) != -1);
		y = g.modPow(x, p);
		return new SecurityKeys(new PrivateKey(p, q, g, y), new PublicKey(x));
	}

	private BigInteger generateP(BigInteger q, int l) {
		if (l % 64 != 0) {
			throw new IllegalArgumentException("L value is wrong");
		}
		BigInteger pTemp;
		BigInteger pTemp2;
		do {
			pTemp = new BigInteger(l, primeCenterie, rand);
			pTemp2 = pTemp.subtract(BigInteger.ONE);
			pTemp = pTemp.subtract(pTemp2.remainder(q));
		} while (!pTemp.isProbablePrime(primeCenterie)
				|| pTemp.bitLength() != l);
		return pTemp;
	}

	private BigInteger generateG(BigInteger p, BigInteger q) {
		BigInteger aux = p.subtract(BigInteger.ONE);
		BigInteger pow = aux.divide(q);
		BigInteger gTemp;
		do {
			gTemp = new BigInteger(aux.bitLength(), rand);
		} while (gTemp.compareTo(aux) != -1
				&& gTemp.compareTo(BigInteger.ONE) != 1);
		return gTemp.modPow(pow, p);
	}

	private BigInteger generateK(BigInteger q) {
		BigInteger tempK;
		do {
			tempK = new BigInteger(q.bitLength(), rand);
		} while (tempK.compareTo(q) != -1
				&& tempK.compareTo(BigInteger.ZERO) != 1);
		return tempK;
	}

	private BigInteger generateR(BigInteger _k, BigInteger _q, BigInteger _p) {
//		k = generateK(q);
		BigInteger rtemp = g.modPow(_k, _p).mod(_q);
		if (rtemp == BigInteger.ZERO) {
			rtemp = BigInteger.ONE;
		}
		return rtemp;
	}

	private BigInteger generateS(BigInteger _r, byte[] data, BigInteger _k, BigInteger _q, BigInteger _x) {
		MessageDigest md;
		BigInteger stemp = BigInteger.ONE;
		try {
			md = MessageDigest.getInstance("SHA-1");
			md.update(data);
			BigInteger hash = new BigInteger(md.digest());
			stemp = (_k.modInverse(_q).multiply(hash.add(_x.multiply(_r)))).mod(_q);
		} catch (NoSuchAlgorithmException ex) {
			// Logger.getLogger(DSA.class.getName()).log(Level.SEVERE, null,
			// ex);
		}
		return stemp;
	}

	public boolean verify(byte[] data, BigInteger r, BigInteger s) {
		if (r.compareTo(BigInteger.ZERO) <= 0 || r.compareTo(q) >= 0) {
			return false;
		}
		if (s.compareTo(BigInteger.ZERO) <= 0 || s.compareTo(q) >= 0) {
			return false;
		}
		MessageDigest md;
		BigInteger v = BigInteger.ZERO;
		try {
			md = MessageDigest.getInstance("SHA-1");
			md.update(data);
			BigInteger hash = new BigInteger(md.digest());
			BigInteger w = s.modInverse(q);
			BigInteger u1 = hash.multiply(w).mod(q);
			BigInteger u2 = r.multiply(w).mod(q);
			v = ((g.modPow(u1, p).multiply(y.modPow(u2, p))).mod(p)).mod(q);
		} catch (NoSuchAlgorithmException ex) {
			// Logger.getLogger(DSA.class.getName()).log(Level.SEVERE, null,
			// ex);
		}
		return v.compareTo(r) == 0;
	}
	
	public SignValues Sing(SecurityKeys keys, byte[] data)
	{
		k = generateK(keys.getPrivate().getQ());
		r = generateR(k, keys.getPrivate().getQ(), keys.getPrivate().getP());
		s = generateS(r, data, k, keys.getPrivate().getQ(), keys.getPublic().getX());
		return new SignValues(s, r);
	}
	
}