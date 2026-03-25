namespace apbd_cw_rental_system_s32529.Exceptions;

public class DomainException(string message) : Exception(message);

public class UserLimitExceededException(string message) : DomainException(message);

public class EquipmentUnavailableException(string message) : DomainException(message);

public class EquipmentNotFoundException(string message) : DomainException(message);