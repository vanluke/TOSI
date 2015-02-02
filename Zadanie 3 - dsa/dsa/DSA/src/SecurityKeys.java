
public class SecurityKeys {
	private PrivateKey Private;
	
	private PublicKey Public;
	
	public PrivateKey getPrivate(){
		return Private;
	}
	
	public PublicKey getPublic() {
		return Public;
	}
	
	public SecurityKeys(PrivateKey privateKey, PublicKey publicKey) {
		this.Public = publicKey;
		this.Private = privateKey;
	}
}
