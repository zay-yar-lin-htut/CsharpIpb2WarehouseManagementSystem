import requests
import sys
import argparse

DEFAULT_MINIMAL_API_PORT = 5192
DEFAULT_CONTROLLER_API_PORT = 5000

passed = 0
failed = 0

def print_result(method, endpoint, status_code, success=True):
    global passed, failed
    if success:
        print(f"  [PASS] {method} {endpoint} -> {status_code}")
        passed += 1
    else:
        print(f"  [FAIL] {method} {endpoint} -> {status_code}")
        failed += 1

def test_products(base_url):
    print("\nTesting Products...")
    
    r = requests.get(f"{base_url}/api/products")
    print_result("GET", "/api/products", r.status_code, r.status_code == 200)
    
    payload = {
        "productName": "Test Product",
        "sku": "SKU001",
        "quantity": 10,
        "unitPrice": 99.99,
        "description": "Test Description"
    }
    r = requests.post(f"{base_url}/api/products", json=payload)
    product_id = r.json().get("productId") if r.status_code == 201 else None
    print_result("POST", "/api/products", r.status_code, r.status_code == 201)
    
    if product_id:
        r = requests.get(f"{base_url}/api/products/{product_id}")
        print_result("GET", f"/api/products/{product_id}", r.status_code, r.status_code == 200)
        
        update_payload = {"productName": "Updated Product", "quantity": 20}
        r = requests.put(f"{base_url}/api/products/{product_id}", json=update_payload)
        print_result("PUT", f"/api/products/{product_id}", r.status_code, r.status_code == 200)
        
        r = requests.delete(f"{base_url}/api/products/{product_id}")
        print_result("DELETE", f"/api/products/{product_id}", r.status_code, r.status_code == 204)

def test_categories(base_url):
    print("\nTesting Categories...")
    
    r = requests.get(f"{base_url}/api/categories")
    print_result("GET", "/api/categories", r.status_code, r.status_code == 200)
    
    payload = {"categoryName": "Test Category", "description": "Test Category Desc"}
    r = requests.post(f"{base_url}/api/categories", json=payload)
    category_id = r.json().get("categoryId") if r.status_code == 201 else None
    print_result("POST", "/api/categories", r.status_code, r.status_code == 201)
    
    if category_id:
        r = requests.get(f"{base_url}/api/categories/{category_id}")
        print_result("GET", f"/api/categories/{category_id}", r.status_code, r.status_code == 200)
        
        update_payload = {"categoryName": "Updated Category"}
        r = requests.put(f"{base_url}/api/categories/{category_id}", json=update_payload)
        print_result("PUT", f"/api/categories/{category_id}", r.status_code, r.status_code == 200)
        
        r = requests.delete(f"{base_url}/api/categories/{category_id}")
        print_result("DELETE", f"/api/categories/{category_id}", r.status_code, r.status_code == 204)

def test_warehouses(base_url):
    print("\nTesting Warehouses...")
    
    r = requests.get(f"{base_url}/api/warehouses")
    print_result("GET", "/api/warehouses", r.status_code, r.status_code == 200)
    
    payload = {"warehouseName": "Test Warehouse", "location": "Test Location", "capacity": 1000}
    r = requests.post(f"{base_url}/api/warehouses", json=payload)
    warehouse_id = r.json().get("warehouseId") if r.status_code == 201 else None
    print_result("POST", "/api/warehouses", r.status_code, r.status_code == 201)
    
    if warehouse_id:
        r = requests.get(f"{base_url}/api/warehouses/{warehouse_id}")
        print_result("GET", f"/api/warehouses/{warehouse_id}", r.status_code, r.status_code == 200)
        
        update_payload = {"warehouseName": "Updated Warehouse", "capacity": 2000}
        r = requests.put(f"{base_url}/api/warehouses/{warehouse_id}", json=update_payload)
        print_result("PUT", f"/api/warehouses/{warehouse_id}", r.status_code, r.status_code == 200)
        
        r = requests.delete(f"{base_url}/api/warehouses/{warehouse_id}")
        print_result("DELETE", f"/api/warehouses/{warehouse_id}", r.status_code, r.status_code == 204)

def test_storers(base_url):
    print("\nTesting Storers...")
    
    r = requests.get(f"{base_url}/api/storers")
    print_result("GET", "/api/storers", r.status_code, r.status_code == 200)
    
    payload = {"fullName": "Test Storer", "email": "test@example.com", "phone": "1234567890"}
    r = requests.post(f"{base_url}/api/storers", json=payload)
    storer_id = r.json().get("storerId") if r.status_code == 201 else None
    print_result("POST", "/api/storers", r.status_code, r.status_code == 201)
    
    if storer_id:
        r = requests.get(f"{base_url}/api/storers/{storer_id}")
        print_result("GET", f"/api/storers/{storer_id}", r.status_code, r.status_code == 200)
        
        update_payload = {"fullName": "Updated Storer"}
        r = requests.put(f"{base_url}/api/storers/{storer_id}", json=update_payload)
        print_result("PUT", f"/api/storers/{storer_id}", r.status_code, r.status_code == 200)
        
        r = requests.delete(f"{base_url}/api/storers/{storer_id}")
        print_result("DELETE", f"/api/storers/{storer_id}", r.status_code, r.status_code == 204)

def main():
    global passed, failed
    
    parser = argparse.ArgumentParser(description="API Test Suite - Warehouse Management System")
    parser.add_argument("--port", type=int, default=DEFAULT_CONTROLLER_API_PORT, 
                        help=f"Port number (default: {DEFAULT_CONTROLLER_API_PORT} for ControllerApi)")
    parser.add_argument("--minimal", action="store_true", 
                        help="Use MinimalApi default port (5192)")
    args = parser.parse_args()
    
    if args.minimal:
        base_url = f"http://localhost:{DEFAULT_MINIMAL_API_PORT}"
    else:
        base_url = f"http://localhost:{args.port}"
    
    print("=" * 60)
    print("API TEST SUITE - Warehouse Management System")
    print("=" * 60)
    print(f"Base URL: {base_url}")
    
    try:
        r = requests.get(f"{base_url}/api/products", timeout=5)
        print(f"\n[INFO] API is running (status: {r.status_code})")
    except requests.exceptions.ConnectionError:
        print("\n[ERROR] Cannot connect to API. Make sure the server is running!")
        print("[ERROR] Run one of:")
        print("  MinimalApi:  dotnet run --project WarehouseManagementSystem.MinimalApi")
        print("  ControllerApi: dotnet run --project WarehouseManagementSystem.ControllerApi")
        sys.exit(1)
    
    test_products(base_url)
    test_categories(base_url)
    test_warehouses(base_url)
    test_storers(base_url)
    
    print("\n" + "=" * 60)
    print(f"RESULTS: PASSED = {passed} | FAILED = {failed}")
    print("=" * 60)
    
    if failed > 0:
        print("\n[RESULT] SOME TESTS FAILED!")
        sys.exit(1)
    else:
        print("\n[RESULT] ALL TESTS PASSED!")
        sys.exit(0)

if __name__ == "__main__":
    main()
